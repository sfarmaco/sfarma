using Sfarma.Api.Models;

namespace Sfarma.Api.DTOs;

public record SaleOrderLineDto(int ProductoId, int Cantidad, decimal PrecioUnitario, decimal Impuestos);
public record SaleOrderDto(int Id, int PartnerId, string Estado, DateTime Fecha, DateTime? FechaEntrega, string Moneda, decimal Total, List<SaleOrderLineItemDto> Lineas);
public record SaleOrderLineItemDto(int Id, int ProductoId, string ProductoNombre, int Cantidad, decimal PrecioUnitario, decimal Impuestos, decimal Subtotal);
public record SaleOrderCreateDto(int PartnerId, DateTime? FechaEntrega, string Moneda, List<SaleOrderLineDto> Lineas);
