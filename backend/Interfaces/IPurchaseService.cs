using Sfarma.Api.DTOs;
using Sfarma.Api.Models;

namespace Sfarma.Api.Interfaces;

public interface IPurchaseService
{
    Task<IEnumerable<PurchaseOrderDto>> GetAllAsync();
    Task<PurchaseOrderDto?> GetByIdAsync(int id);
    Task<PurchaseOrderDto> CreateAsync(PurchaseOrderCreateDto dto);
    Task<bool> UpdateStateAsync(int id, PurchaseOrderState state);
}
