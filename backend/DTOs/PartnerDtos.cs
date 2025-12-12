namespace Sfarma.Api.DTOs;

public record PartnerDto(int Id, string Nombre, string? Tipo, string? Email, string? Telefono, string? Direccion, string? Ciudad, string? Pais, bool EsCliente, bool EsProveedor);
public record PartnerCreateDto(string Nombre, string? Tipo, string? Email, string? Telefono, string? Direccion, string? Ciudad, string? Pais, bool EsCliente = true, bool EsProveedor = false);
