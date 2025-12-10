namespace Sfarma.Api.DTOs;

public record DetalleVentaCreateDto(int ProductoId, int Cantidad, decimal PrecioUnitario);

public record VentaCreateDto(DateTime Fecha, string TipoVenta, int EmpleadoId, int? ClienteId, List<DetalleVentaCreateDto> Detalles);

public record DetalleVentaDto(int Id, int ProductoId, int Cantidad, decimal PrecioUnitario);

public record VentaDto(int Id, DateTime Fecha, string TipoVenta, int EmpleadoId, int? ClienteId, decimal Total, List<DetalleVentaDto> Detalles);
