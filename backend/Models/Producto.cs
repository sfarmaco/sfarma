namespace Sfarma.Api.Models;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string PrincipioActivo { get; set; } = null!;
    public string Laboratorio { get; set; } = null!;
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public int StockActual { get; set; }
    public int StockMinimo { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string Lote { get; set; } = null!;
    public string CodigoBarras { get; set; } = null!;
    public ICollection<DetalleVenta>? DetallesVenta { get; set; }
}
