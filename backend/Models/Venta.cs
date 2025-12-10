namespace Sfarma.Api.Models;

public class Venta
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public string TipoVenta { get; set; } = "Contado";
    public int EmpleadoId { get; set; }
    public int? ClienteId { get; set; }
    public Usuario? Empleado { get; set; }
    public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
}
