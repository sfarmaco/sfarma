using Sfarma.Api.DTOs;

namespace Sfarma.Api.Interfaces;

public interface IVentaService
{
    Task<IEnumerable<VentaDto>> GetAllAsync();
    Task<VentaDto?> GetByIdAsync(int id);
    Task<VentaDto> CreateAsync(VentaCreateDto dto);
}
