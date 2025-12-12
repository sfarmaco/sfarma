namespace Sfarma.Api.Models;

public class Partner
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Tipo { get; set; } // persona / empresa
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? Ciudad { get; set; }
    public string? Pais { get; set; }
    public bool EsCliente { get; set; } = true;
    public bool EsProveedor { get; set; } = false;

    public ICollection<SaleOrder>? SaleOrders { get; set; }
}
