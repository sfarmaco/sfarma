using Sfarma.Api.DTOs;
using Sfarma.Api.Models;

namespace Sfarma.Api.Interfaces;

public interface ISaleOrderService
{
    Task<IEnumerable<SaleOrderDto>> GetAllAsync();
    Task<SaleOrderDto?> GetByIdAsync(int id);
    Task<SaleOrderDto> CreateAsync(SaleOrderCreateDto dto);
    Task<bool> UpdateStateAsync(int id, SaleOrderState nuevoEstado);
}
