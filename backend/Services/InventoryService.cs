using Microsoft.EntityFrameworkCore;
using Sfarma.Api.Data;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;
using Sfarma.Api.Models;

namespace Sfarma.Api.Services;

public class InventoryService : IInventoryService
{
    private readonly SfarmaContext _context;
    public InventoryService(SfarmaContext context)
    {
        _context = context;
    }

    public async Task<LocationDto> CreateLocationAsync(LocationCreateDto dto)
    {
        var loc = new Location
        {
            Nombre = dto.Nombre,
            Tipo = Enum.TryParse<LocationType>(dto.Tipo, true, out var t) ? t : LocationType.Internal,
            ParentId = dto.ParentId
        };
        _context.Locations.Add(loc);
        await _context.SaveChangesAsync();
        return new LocationDto(loc.Id, loc.Nombre, loc.Tipo.ToString(), loc.ParentId);
    }

    public async Task<IEnumerable<LocationDto>> GetLocationsAsync()
    {
        var list = await _context.Locations.ToListAsync();
        return list.Select(l => new LocationDto(l.Id, l.Nombre, l.Tipo.ToString(), l.ParentId));
    }

    public async Task<PickingDto> CreatePickingAsync(PickingCreateDto dto)
    {
        var picking = new Picking
        {
            Tipo = Enum.TryParse<PickingType>(dto.Tipo, true, out var t) ? t : PickingType.Outgoing,
            Estado = PickingState.Draft,
            OrigenId = dto.OrigenId,
            DestinoId = dto.DestinoId
        };
        foreach (var m in dto.Movimientos)
        {
            picking.Movimientos.Add(new Move
            {
                ProductoId = m.ProductoId,
                Cantidad = m.Cantidad,
                Estado = PickingState.Draft,
                LoteId = m.LoteId
            });
        }
        _context.Pickings.Add(picking);
        await _context.SaveChangesAsync();
        return Map(picking);
    }

    public async Task<IEnumerable<PickingDto>> GetPickingsAsync()
    {
        var list = await _context.Pickings.Include(p => p.Movimientos).OrderByDescending(p => p.Id).ToListAsync();
        return list.Select(Map);
    }

    public async Task<PickingDto?> GetPickingByIdAsync(int id)
    {
        var p = await _context.Pickings.Include(x => x.Movimientos).FirstOrDefaultAsync(x => x.Id == id);
        return p is null ? null : Map(p);
    }

    public async Task<bool> UpdatePickingStateAsync(int id, PickingState state)
    {
        var p = await _context.Pickings.Include(x => x.Movimientos).FirstOrDefaultAsync(x => x.Id == id);
        if (p is null) return false;
        p.Estado = state;
        foreach (var m in p.Movimientos)
        {
            m.Estado = state;
            if (state == PickingState.Done)
            {
                var prod = await _context.Productos.FirstOrDefaultAsync(pr => pr.Id == m.ProductoId);
                if (prod is not null)
                {
                    if (p.Tipo == PickingType.Outgoing)
                        prod.StockActual = Math.Max(0, prod.StockActual - (int)m.Cantidad);
                    else if (p.Tipo == PickingType.Incoming)
                        prod.StockActual += (int)m.Cantidad;
                }
            }
        }
        await _context.SaveChangesAsync();
        return true;
    }

    private static PickingDto Map(Picking p)
    {
        var moves = p.Movimientos.Select(m => new MoveDto(m.Id, m.ProductoId, m.Cantidad, m.Estado.ToString(), m.LoteId)).ToList();
        return new PickingDto(p.Id, p.Tipo.ToString(), p.Estado.ToString(), p.OrigenId, p.DestinoId, moves);
    }
}
