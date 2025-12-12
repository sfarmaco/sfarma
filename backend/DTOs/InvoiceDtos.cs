using Sfarma.Api.Models;

namespace Sfarma.Api.DTOs;

public record InvoiceLineDto(int ProductoId, int Cantidad, decimal PrecioUnitario, decimal Impuestos);
public record InvoiceCreateDto(int PartnerId, InvoiceType Tipo, string Moneda, List<InvoiceLineDto> Lineas);
public record InvoiceLineItemDto(int Id, int ProductoId, string ProductoNombre, int Cantidad, decimal PrecioUnitario, decimal Impuestos, decimal Subtotal);
public record InvoiceDto(int Id, int PartnerId, string Tipo, string Estado, DateTime Fecha, string Moneda, decimal Total, List<InvoiceLineItemDto> Lineas);
