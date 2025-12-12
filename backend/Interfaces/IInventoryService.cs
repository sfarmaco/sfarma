using Sfarma.Api.DTOs;
using Sfarma.Api.Models;

namespace Sfarma.Api.Interfaces;

public interface IInventoryService
{
    Task<IEnumerable<LocationDto>> GetLocationsAsync();
    Task<LocationDto> CreateLocationAsync(LocationCreateDto dto);
    Task<IEnumerable<PickingDto>> GetPickingsAsync();
    Task<PickingDto?> GetPickingByIdAsync(int id);
    Task<PickingDto> CreatePickingAsync(PickingCreateDto dto);
    Task<bool> UpdatePickingStateAsync(int id, PickingState state);
}
