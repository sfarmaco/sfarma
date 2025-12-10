using Microsoft.AspNetCore.Identity;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;
using Sfarma.Api.Models;

namespace Sfarma.Api.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IGenericRepository<Usuario> _repo;
    private readonly IPasswordHasher<Usuario> _hasher;
    public UsuarioService(IGenericRepository<Usuario> repo, IPasswordHasher<Usuario> hasher)
    {
        _repo = repo; _hasher = hasher;
    }

    public async Task<UsuarioDto> CreateAsync(UsuarioCreateDto dto)
    {
        var entity = new Usuario { Nombre = dto.Nombre, Email = dto.Email, Rol = dto.Rol };
        entity.PasswordHash = _hasher.HashPassword(entity, dto.Password);
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return Map(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return false;
        await _repo.DeleteAsync(entity);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<UsuarioDto>> GetAllAsync() => (await _repo.GetAllAsync()).Select(Map);

    public async Task<UsuarioDto?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    private static UsuarioDto Map(Usuario u) => new(u.Id, u.Nombre, u.Email, u.Rol);
}
