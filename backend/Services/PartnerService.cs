using Sfarma.Api.DTOs;
using Sfarma.Api.Interfaces;
using Sfarma.Api.Models;

namespace Sfarma.Api.Services;

public class PartnerService : IPartnerService
{
    private readonly IGenericRepository<Partner> _repo;

    public PartnerService(IGenericRepository<Partner> repo)
    {
        _repo = repo;
    }

    public async Task<PartnerDto> CreateAsync(PartnerCreateDto dto)
    {
        var entity = new Partner
        {
            Nombre = dto.Nombre,
            Tipo = dto.Tipo,
            Email = dto.Email,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            Ciudad = dto.Ciudad,
            Pais = dto.Pais,
            EsCliente = dto.EsCliente,
            EsProveedor = dto.EsProveedor
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

    public async Task<IEnumerable<PartnerDto>> GetAllAsync() =>
        (await _repo.GetAllAsync()).Select(Map);

    public async Task<PartnerDto?> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<bool> UpdateAsync(int id, PartnerCreateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return false;
        entity.Nombre = dto.Nombre;
        entity.Tipo = dto.Tipo;
        entity.Email = dto.Email;
        entity.Telefono = dto.Telefono;
        entity.Direccion = dto.Direccion;
        entity.Ciudad = dto.Ciudad;
        entity.Pais = dto.Pais;
        entity.EsCliente = dto.EsCliente;
        entity.EsProveedor = dto.EsProveedor;
        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();
        return true;
    }

    private static PartnerDto Map(Partner p) =>
        new(p.Id, p.Nombre, p.Tipo, p.Email, p.Telefono, p.Direccion, p.Ciudad, p.Pais, p.EsCliente, p.EsProveedor);
}
