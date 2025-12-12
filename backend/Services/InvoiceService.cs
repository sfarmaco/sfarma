using Microsoft.EntityFrameworkCore;
using Sfarma.Api.Data;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;
using Sfarma.Api.Models;

namespace Sfarma.Api.Services;

public class InvoiceService : IInvoiceService
{
    private readonly SfarmaContext _context;
    public InvoiceService(SfarmaContext context)
    {
        _context = context;
    }

    public async Task<InvoiceDto> CreateAsync(InvoiceCreateDto dto)
    {
        var inv = new Invoice
        {
            PartnerId = dto.PartnerId,
            Tipo = dto.Tipo,
            Estado = InvoiceState.Draft,
            Moneda = string.IsNullOrWhiteSpace(dto.Moneda) ? "USD" : dto.Moneda,
            Fecha = DateTime.UtcNow
        };
        foreach (var l in dto.Lineas)
        {
            var subtotal = l.Cantidad * l.PrecioUnitario + l.Impuestos;
            inv.Lineas.Add(new InvoiceLine
            {
                ProductoId = l.ProductoId,
                Cantidad = l.Cantidad,
                PrecioUnitario = l.PrecioUnitario,
                Impuestos = l.Impuestos,
                Subtotal = subtotal
            });
        }
        inv.Total = inv.Lineas.Sum(l => l.Subtotal);
        _context.Invoices.Add(inv);
        await _context.SaveChangesAsync();
        return Map(inv);
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllAsync()
    {
        var data = await _context.Invoices.Include(i => i.Lineas).OrderByDescending(i => i.Id).ToListAsync();
        return data.Select(Map);
    }

    public async Task<InvoiceDto?> GetByIdAsync(int id)
    {
        var inv = await _context.Invoices.Include(i => i.Lineas).FirstOrDefaultAsync(i => i.Id == id);
        return inv is null ? null : Map(inv);
    }

    public async Task<bool> UpdateStateAsync(int id, InvoiceState state)
    {
        var inv = await _context.Invoices.FindAsync(id);
        if (inv is null) return false;
        inv.Estado = state;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<InvoiceDto?> CreateFromSaleOrderAsync(int saleOrderId)
    {
        var order = await _context.SaleOrders
            .Include(o => o.Lineas)
            .FirstOrDefaultAsync(o => o.Id == saleOrderId);
        if (order is null) return null;

        var inv = new Invoice
        {
            PartnerId = order.PartnerId,
            Tipo = InvoiceType.Customer,
            Estado = InvoiceState.Open,
            Moneda = order.Moneda,
            Fecha = DateTime.UtcNow
        };

        foreach (var l in order.Lineas)
        {
            var subtotal = l.Cantidad * l.PrecioUnitario + l.Impuestos;
            inv.Lineas.Add(new InvoiceLine
            {
                ProductoId = l.ProductoId,
                Cantidad = l.Cantidad,
                PrecioUnitario = l.PrecioUnitario,
                Impuestos = l.Impuestos,
                Subtotal = subtotal
            });
        }
        inv.Total = inv.Lineas.Sum(l => l.Subtotal);
        _context.Invoices.Add(inv);
        order.Estado = SaleOrderState.Invoiced;
        await _context.SaveChangesAsync();
        return Map(inv);
    }

    public async Task<InvoiceDto?> CreateFromPurchaseOrderAsync(int purchaseOrderId)
    {
        var order = await _context.PurchaseOrders
            .Include(o => o.Lineas)
            .FirstOrDefaultAsync(o => o.Id == purchaseOrderId);
        if (order is null) return null;

        var inv = new Invoice
        {
            PartnerId = order.ProveedorId, // proveedor como partner
            Tipo = InvoiceType.Vendor,
            Estado = InvoiceState.Open,
            Moneda = order.Moneda,
            Fecha = DateTime.UtcNow
        };

        foreach (var l in order.Lineas)
        {
            var subtotal = l.Cantidad * l.PrecioUnitario + l.Impuestos;
            inv.Lineas.Add(new InvoiceLine
            {
                ProductoId = l.ProductoId,
                Cantidad = l.Cantidad,
                PrecioUnitario = l.PrecioUnitario,
                Impuestos = l.Impuestos,
                Subtotal = subtotal
            });
        }
        inv.Total = inv.Lineas.Sum(l => l.Subtotal);
        _context.Invoices.Add(inv);
        order.Estado = PurchaseOrderState.Invoiced;
        await _context.SaveChangesAsync();
        return Map(inv);
    }

    private static InvoiceDto Map(Invoice i)
    {
        var lineas = i.Lineas.Select(l =>
            new InvoiceLineItemDto(l.Id, l.ProductoId, l.Producto?.Nombre ?? string.Empty, l.Cantidad, l.PrecioUnitario, l.Impuestos, l.Subtotal)
        ).ToList();
        return new InvoiceDto(i.Id, i.PartnerId, i.Tipo.ToString(), i.Estado.ToString(), i.Fecha, i.Moneda, i.Total, lineas);
    }
}
