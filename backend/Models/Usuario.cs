namespace Sfarma.Api.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Rol { get; set; } = "Empleado";
    public ICollection<Venta>? Ventas { get; set; }
}
