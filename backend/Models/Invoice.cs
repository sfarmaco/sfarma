namespace Sfarma.Api.Models;

public enum InvoiceState
{
    Draft,
    Open,
    Paid,
    Cancelled
}

public enum InvoiceType
{
    Customer,
    Vendor
}

public class Invoice
{
    public int Id { get; set; }
    public int PartnerId { get; set; }
    public Partner? Partner { get; set; }
    public InvoiceType Tipo { get; set; } = InvoiceType.Customer;
    public InvoiceState Estado { get; set; } = InvoiceState.Draft;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string Moneda { get; set; } = "USD";
    public decimal Total { get; set; }
    public ICollection<InvoiceLine> Lineas { get; set; } = new List<InvoiceLine>();
}

public class InvoiceLine
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Impuestos { get; set; }
    public decimal Subtotal { get; set; }
}
