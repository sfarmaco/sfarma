namespace Sfarma.Api.Models;

public enum LocationType
{
    Internal,
    Customer,
    Supplier,
    Transit
}

public enum PickingType
{
    Incoming,
    Outgoing,
    Internal
}

public enum PickingState
{
    Draft,
    Reserved,
    Done
}

public class Location
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public LocationType Tipo { get; set; } = LocationType.Internal;
    public int? ParentId { get; set; }
    public Location? Parent { get; set; }
}

public class Lot
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public string LoteCodigo { get; set; } = null!;
    public DateTime? FechaVencimiento { get; set; }
    public decimal CantidadDisponible { get; set; }
}

public class Picking
{
    public int Id { get; set; }
    public PickingType Tipo { get; set; } = PickingType.Outgoing;
    public PickingState Estado { get; set; } = PickingState.Draft;
    public int? OrigenId { get; set; }
    public Location? Origen { get; set; }
    public int? DestinoId { get; set; }
    public Location? Destino { get; set; }
    public ICollection<Move> Movimientos { get; set; } = new List<Move>();
}

public class Move
{
    public int Id { get; set; }
    public int PickingId { get; set; }
    public Picking? Picking { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public decimal Cantidad { get; set; }
    public PickingState Estado { get; set; } = PickingState.Draft;
    public int? LoteId { get; set; }
    public Lot? Lote { get; set; }
}
