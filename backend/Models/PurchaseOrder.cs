namespace Sfarma.Api.Models;

public enum PurchaseOrderState
{
    Draft,
    Sent,
    Received,
    Invoiced,
    Paid,
    Cancelled
}

public class PurchaseOrder
{
    public int Id { get; set; }
    public int ProveedorId { get; set; }
    public Proveedor? Proveedor { get; set; }
    public PurchaseOrderState Estado { get; set; } = PurchaseOrderState.Draft;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public DateTime? FechaEntrega { get; set; }
    public string Moneda { get; set; } = "USD";
    public decimal Total { get; set; }
    public ICollection<PurchaseOrderLine> Lineas { get; set; } = new List<PurchaseOrderLine>();
}

public class PurchaseOrderLine
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Impuestos { get; set; }
    public decimal Subtotal { get; set; }
}
