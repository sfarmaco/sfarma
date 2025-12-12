using Sfarma.Api.Models;

namespace Sfarma.Api.DTOs;

public record LocationDto(int Id, string Nombre, string Tipo, int? ParentId);
public record LocationCreateDto(string Nombre, string Tipo, int? ParentId);

public record MoveDto(int Id, int ProductoId, decimal Cantidad, string Estado, int? LoteId);
public record MoveCreateDto(int ProductoId, decimal Cantidad, int? LoteId);

public record PickingCreateDto(string Tipo, int? OrigenId, int? DestinoId, List<MoveCreateDto> Movimientos);
public record PickingDto(int Id, string Tipo, string Estado, int? OrigenId, int? DestinoId, List<MoveDto> Movimientos);
