namespace Sfarma.Api.Models;

public enum SaleOrderState
{
    Quote,
    Confirmed,
    Delivered,
    Invoiced,
    Paid,
    Cancelled
}

public class SaleOrder
{
    public int Id { get; set; }
    public int PartnerId { get; set; }
    public Partner? Partner { get; set; }
    public SaleOrderState Estado { get; set; } = SaleOrderState.Quote;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public DateTime? FechaEntrega { get; set; }
    public string Moneda { get; set; } = "USD";
    public decimal Total { get; set; }
    public ICollection<SaleOrderLine> Lineas { get; set; } = new List<SaleOrderLine>();
}

public class SaleOrderLine
{
    public int Id { get; set; }
    public int SaleOrderId { get; set; }
    public SaleOrder? SaleOrder { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Impuestos { get; set; }
    public decimal Subtotal { get; set; }
}
