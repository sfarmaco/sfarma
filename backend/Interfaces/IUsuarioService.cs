using Sfarma.Api.DTOs;

namespace Sfarma.Api.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> GetAllAsync();
    Task<UsuarioDto?> GetByIdAsync(int id);
    Task<UsuarioDto> CreateAsync(UsuarioCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
