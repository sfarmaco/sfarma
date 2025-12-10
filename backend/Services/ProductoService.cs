using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;
using Sfarma.Api.Models;

namespace Sfarma.Api.Services;

public class ProductoService : IProductoService
{
    private readonly IGenericRepository<Producto> _repo;
    public ProductoService(IGenericRepository<Producto> repo) => _repo = repo;

    public async Task<ProductoDto> CreateAsync(ProductoCreateDto dto)
    {
        var entity = new Producto
        {
            Nombre = dto.Nombre,
            PrincipioActivo = dto.PrincipioActivo,
            Laboratorio = dto.Laboratorio,
            PrecioCompra = dto.PrecioCompra,
            PrecioVenta = dto.PrecioVenta,
            StockActual = dto.StockActual,
            StockMinimo = dto.StockMinimo,
            FechaVencimiento = dto.FechaVencimiento,
            Lote = dto.Lote,
            CodigoBarras = dto.CodigoBarras
        };
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return Map(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return false;
        await _repo.DeleteAsync(entity);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ProductoDto>> GetAllAsync() =>
        (await _repo.GetAllAsync()).Select(Map);

    public async Task<ProductoDto?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<bool> UpdateAsync(int id, ProductoCreateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return false;
        entity.Nombre = dto.Nombre;
        entity.PrincipioActivo = dto.PrincipioActivo;
        entity.Laboratorio = dto.Laboratorio;
        entity.PrecioCompra = dto.PrecioCompra;
        entity.PrecioVenta = dto.PrecioVenta;
        entity.StockActual = dto.StockActual;
        entity.StockMinimo = dto.StockMinimo;
        entity.FechaVencimiento = dto.FechaVencimiento;
        entity.Lote = dto.Lote;
        entity.CodigoBarras = dto.CodigoBarras;
        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static ProductoDto Map(Producto p) => new(p.Id, p.Nombre, p.PrincipioActivo, p.Laboratorio,
        p.PrecioCompra, p.PrecioVenta, p.StockActual, p.StockMinimo, p.FechaVencimiento, p.Lote, p.CodigoBarras);
}
