namespace Sfarma.Api.DTOs;

public record UsuarioDto(int Id, string Nombre, string Email, string Rol);
public record UsuarioCreateDto(string Nombre, string Email, string Password, string Rol);
