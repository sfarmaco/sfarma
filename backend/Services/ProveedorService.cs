using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;
using Sfarma.Api.Models;

namespace Sfarma.Api.Services;

public class ProveedorService : IProveedorService
{
    private readonly IGenericRepository<Proveedor> _repo;
    public ProveedorService(IGenericRepository<Proveedor> repo) => _repo = repo;

    public async Task<ProveedorDto> CreateAsync(ProveedorCreateDto dto)
    {
        var entity = new Proveedor { Nombre = dto.Nombre, Contacto = dto.Contacto, Direccion = dto.Direccion };
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

    public async Task<IEnumerable<ProveedorDto>> GetAllAsync() => (await _repo.GetAllAsync()).Select(Map);

    public async Task<ProveedorDto?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<bool> UpdateAsync(int id, ProveedorCreateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return false;
        entity.Nombre = dto.Nombre;
        entity.Contacto = dto.Contacto;
        entity.Direccion = dto.Direccion;
        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static ProveedorDto Map(Proveedor p) => new(p.Id, p.Nombre, p.Contacto, p.Direccion);
}
