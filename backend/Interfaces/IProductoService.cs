using Sfarma.Api.DTOs;

namespace Sfarma.Api.Interfaces;

public interface IProductoService
{
    Task<IEnumerable<ProductoDto>> GetAllAsync();
    Task<ProductoDto?> GetByIdAsync(int id);
    Task<ProductoDto> CreateAsync(ProductoCreateDto dto);
    Task<bool> UpdateAsync(int id, ProductoCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
