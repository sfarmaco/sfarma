using Sfarma.Api.DTOs;

namespace Sfarma.Api.Interfaces;

public interface IAuthService
{
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(UsuarioCreateDto request);
}
