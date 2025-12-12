using Microsoft.EntityFrameworkCore;
using Sfarma.Api.Data;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;
using Sfarma.Api.Models;

namespace Sfarma.Api.Services;

public class PurchaseService : IPurchaseService
{
    private readonly SfarmaContext _context;
    public PurchaseService(SfarmaContext context)
    {
        _context = context;
    }

    public async Task<PurchaseOrderDto> CreateAsync(PurchaseOrderCreateDto dto)
    {
        var order = new PurchaseOrder
        {
            ProveedorId = dto.ProveedorId,
            Fecha = DateTime.UtcNow,
            FechaEntrega = dto.FechaEntrega,
            Moneda = string.IsNullOrWhiteSpace(dto.Moneda) ? "USD" : dto.Moneda
        };
        foreach (var line in dto.Lineas)
        {
            var subtotal = line.Cantidad * line.PrecioUnitario + line.Impuestos;
            order.Lineas.Add(new PurchaseOrderLine
            {
                ProductoId = line.ProductoId,
                Cantidad = line.Cantidad,
                PrecioUnitario = line.PrecioUnitario,
                Impuestos = line.Impuestos,
                Subtotal = subtotal
            });
        }
        order.Total = order.Lineas.Sum(l => l.Subtotal);
        _context.PurchaseOrders.Add(order);
        await _context.SaveChangesAsync();
        return Map(order);
    }

    public async Task<IEnumerable<PurchaseOrderDto>> GetAllAsync()
    {
        var orders = await _context.PurchaseOrders.Include(o => o.Lineas).OrderByDescending(o => o.Id).ToListAsync();
        return orders.Select(Map);
    }

    public async Task<PurchaseOrderDto?> GetByIdAsync(int id)
    {
        var order = await _context.PurchaseOrders.Include(o => o.Lineas).FirstOrDefaultAsync(o => o.Id == id);
        return order is null ? null : Map(order);
    }

    public async Task<bool> UpdateStateAsync(int id, PurchaseOrderState state)
    {
        var order = await _context.PurchaseOrders
            .Include(o => o.Lineas)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (order is null) return false;

        if (order.Estado != PurchaseOrderState.Received && state == PurchaseOrderState.Received)
        {
            // Sumar stock al recibir
            foreach (var line in order.Lineas)
            {
                var prod = await _context.Productos.FirstOrDefaultAsync(p => p.Id == line.ProductoId);
                if (prod is not null)
                {
                    prod.StockActual += line.Cantidad;
                }
            }
        }

        order.Estado = state;
        await _context.SaveChangesAsync();
        return true;
    }

    private static PurchaseOrderDto Map(PurchaseOrder o)
    {
        var lineas = o.Lineas.Select(l =>
            new PurchaseOrderLineItemDto(l.Id, l.ProductoId, l.Producto?.Nombre ?? string.Empty, l.Cantidad, l.PrecioUnitario, l.Impuestos, l.Subtotal)
        ).ToList();
        return new PurchaseOrderDto(o.Id, o.ProveedorId, o.Estado.ToString(), o.Fecha, o.FechaEntrega, o.Moneda, o.Total, lineas);
    }
}
