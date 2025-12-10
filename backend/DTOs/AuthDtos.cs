namespace Sfarma.Api.DTOs;

public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, UsuarioDto Usuario);
