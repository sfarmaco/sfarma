namespace Sfarma.Api.DTOs;

public record ProveedorDto(int Id, string Nombre, string Contacto, string Direccion);
public record ProveedorCreateDto(string Nombre, string Contacto, string Direccion);
