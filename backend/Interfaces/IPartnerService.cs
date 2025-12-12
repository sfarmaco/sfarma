using Sfarma.Api.DTOs;

namespace Sfarma.Api.Interfaces;

public interface IPartnerService
{
    Task<IEnumerable<PartnerDto>> GetAllAsync();
    Task<PartnerDto?> GetByIdAsync(int id);
    Task<PartnerDto> CreateAsync(PartnerCreateDto dto);
    Task<bool> UpdateAsync(int id, PartnerCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
