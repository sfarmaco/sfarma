using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;
using Sfarma.Api.Models;

namespace Sfarma.Api.Services;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<Usuario> _repo;
    private readonly IPasswordHasher<Usuario> _hasher;
    private readonly IConfiguration _config;

    public AuthService(IGenericRepository<Usuario> repo, IPasswordHasher<Usuario> hasher, IConfiguration config)
    {
        _repo = repo; _hasher = hasher; _config = config;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = (await _repo.FindAsync(u => u.Email == request.Email)).FirstOrDefault();
        if (user is null) return null;
        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed) return null;
        return new AuthResponse(GenerateToken(user), new UsuarioDto(user.Id, user.Nombre, user.Email, user.Rol));
    }

    public async Task<AuthResponse> RegisterAsync(UsuarioCreateDto dto)
    {
        var entity = new Usuario { Nombre = dto.Nombre, Email = dto.Email, Rol = dto.Rol };
        entity.PasswordHash = _hasher.HashPassword(entity, dto.Password);
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return new AuthResponse(GenerateToken(entity), new UsuarioDto(entity.Id, entity.Nombre, entity.Email, entity.Rol));
    }

    private string GenerateToken(Usuario user)
    {
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new("uid", user.Id.ToString()),
            new(ClaimTypes.Role, user.Rol),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
