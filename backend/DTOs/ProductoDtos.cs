namespace Sfarma.Api.DTOs;

public record ProductoDto(int Id, string Nombre, string PrincipioActivo, string Laboratorio,
    decimal PrecioCompra, decimal PrecioVenta, int StockActual, int StockMinimo,
    DateTime FechaVencimiento, string Lote, string CodigoBarras);

public record ProductoCreateDto(string Nombre, string PrincipioActivo, string Laboratorio,
    decimal PrecioCompra, decimal PrecioVenta, int StockActual, int StockMinimo,
    DateTime FechaVencimiento, string Lote, string CodigoBarras);
