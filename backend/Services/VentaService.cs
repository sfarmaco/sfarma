using Microsoft.EntityFrameworkCore;
using Sfarma.Api.Data;
using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;
using Sfarma.Api.Models;

namespace Sfarma.Api.Services;

public class VentaService : IVentaService
{
    private readonly SfarmaContext _context;
    private readonly IGenericRepository<Venta> _repoVenta;
    public VentaService(SfarmaContext context, IGenericRepository<Venta> repoVenta)
    {
        _context = context;
        _repoVenta = repoVenta;
    }

    public async Task<VentaDto> CreateAsync(VentaCreateDto dto)
    {
        var venta = new Venta
        {
            Fecha = dto.Fecha,
            TipoVenta = dto.TipoVenta,
            EmpleadoId = dto.EmpleadoId,
            ClienteId = dto.ClienteId
        };

        foreach (var det in dto.Detalles)
        {
            venta.Detalles.Add(new DetalleVenta
            {
                ProductoId = det.ProductoId,
                Cantidad = det.Cantidad,
                PrecioUnitario = det.PrecioUnitario
            });
        }

        venta.Total = venta.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);

        await _repoVenta.AddAsync(venta);
        await _repoVenta.SaveChangesAsync();
        return Map(venta);
    }

    public async Task<IEnumerable<VentaDto>> GetAllAsync()
    {
        var ventas = await _context.Ventas.Include(v => v.Detalles).ToListAsync();
        return ventas.Select(Map);
    }

    public async Task<VentaDto?> GetByIdAsync(int id)
    {
        var venta = await _context.Ventas.Include(v => v.Detalles).FirstOrDefaultAsync(v => v.Id == id);
        return venta is null ? null : Map(venta);
    }

    private static VentaDto Map(Venta v) => new(
        v.Id, v.Fecha, v.TipoVenta, v.EmpleadoId, v.ClienteId, v.Total,
        v.Detalles.Select(d => new DetalleVentaDto(d.Id, d.ProductoId, d.Cantidad, d.PrecioUnitario)).ToList());
}
