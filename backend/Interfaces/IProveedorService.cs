using Sfarma.Api.DTOs;

namespace Sfarma.Api.Interfaces;

public interface IProveedorService
{
    Task<IEnumerable<ProveedorDto>> GetAllAsync();
    Task<ProveedorDto?> GetByIdAsync(int id);
    Task<ProveedorDto> CreateAsync(ProveedorCreateDto dto);
    Task<bool> UpdateAsync(int id, ProveedorCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
