using Microsoft.EntityFrameworkCore;
using Sfarma.Api.Data;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;
using Sfarma.Api.Models;

namespace Sfarma.Api.Services;

public class SaleOrderService : ISaleOrderService
{
    private readonly SfarmaContext _context;

    public SaleOrderService(SfarmaContext context)
    {
        _context = context;
    }

    public async Task<SaleOrderDto> CreateAsync(SaleOrderCreateDto dto)
    {
        var order = new SaleOrder
        {
            PartnerId = dto.PartnerId,
            Fecha = DateTime.UtcNow,
            FechaEntrega = dto.FechaEntrega,
            Moneda = string.IsNullOrWhiteSpace(dto.Moneda) ? "USD" : dto.Moneda
        };

        foreach (var line in dto.Lineas)
        {
            var subtotal = line.Cantidad * line.PrecioUnitario + line.Impuestos;
            order.Lineas.Add(new SaleOrderLine
            {
                ProductoId = line.ProductoId,
                Cantidad = line.Cantidad,
                PrecioUnitario = line.PrecioUnitario,
                Impuestos = line.Impuestos,
                Subtotal = subtotal
            });
        }
        order.Total = order.Lineas.Sum(l => l.Subtotal);

        _context.SaleOrders.Add(order);
        await _context.SaveChangesAsync();
        return Map(order);
    }

    public async Task<IEnumerable<SaleOrderDto>> GetAllAsync()
    {
        var orders = await _context.SaleOrders
            .Include(o => o.Lineas)
            .Include(o => o.Partner)
            .OrderByDescending(o => o.Id)
            .ToListAsync();
        return orders.Select(Map);
    }

    public async Task<SaleOrderDto?> GetByIdAsync(int id)
    {
        var order = await _context.SaleOrders
            .Include(o => o.Lineas)
            .Include(o => o.Partner)
            .FirstOrDefaultAsync(o => o.Id == id);
        return order is null ? null : Map(order);
    }

    public async Task<bool> UpdateStateAsync(int id, SaleOrderState nuevoEstado)
    {
        var order = await _context.SaleOrders
            .Include(o => o.Lineas)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (order is null) return false;

        // Evita descontar stock más de una vez si ya estaba entregado
        if (order.Estado != SaleOrderState.Delivered && nuevoEstado == SaleOrderState.Delivered)
        {
            foreach (var line in order.Lineas)
            {
                var prod = await _context.Productos.FirstOrDefaultAsync(p => p.Id == line.ProductoId);
                if (prod is not null)
                {
                    prod.StockActual = Math.Max(0, prod.StockActual - line.Cantidad);
                }
            }
        }

        order.Estado = nuevoEstado;
        await _context.SaveChangesAsync();
        return true;
    }

    private static SaleOrderDto Map(SaleOrder o)
    {
        var lineas = o.Lineas.Select(l =>
            new SaleOrderLineItemDto(l.Id, l.ProductoId, l.Producto?.Nombre ?? string.Empty, l.Cantidad, l.PrecioUnitario, l.Impuestos, l.Subtotal)
        ).ToList();
        return new SaleOrderDto(o.Id, o.PartnerId, o.Estado.ToString(), o.Fecha, o.FechaEntrega, o.Moneda, o.Total, lineas);
    }
}
