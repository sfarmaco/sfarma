namespace Sfarma.Api.Models;

public class Proveedor
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Contacto { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public ICollection<Producto>? Productos { get; set; }
}
