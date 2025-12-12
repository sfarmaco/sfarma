using Sfarma.Api.Models;

namespace Sfarma.Api.DTOs;

public record PurchaseOrderLineDto(int ProductoId, int Cantidad, decimal PrecioUnitario, decimal Impuestos);
public record PurchaseOrderCreateDto(int ProveedorId, DateTime? FechaEntrega, string Moneda, List<PurchaseOrderLineDto> Lineas);
public record PurchaseOrderLineItemDto(int Id, int ProductoId, string ProductoNombre, int Cantidad, decimal PrecioUnitario, decimal Impuestos, decimal Subtotal);
public record PurchaseOrderDto(int Id, int ProveedorId, string Estado, DateTime Fecha, DateTime? FechaEntrega, string Moneda, decimal Total, List<PurchaseOrderLineItemDto> Lineas);
