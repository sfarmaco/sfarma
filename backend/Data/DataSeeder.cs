using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Sfarma.Api.Models;

namespace Sfarma.Api.Data;

public class DataSeeder
{
    private readonly SfarmaContext _context;
    private readonly IPasswordHasher<Usuario> _hasher;

    public DataSeeder(SfarmaContext context, IPasswordHasher<Usuario> hasher)
    {
        _context = context;
        _hasher = hasher;
    }

    public async Task SeedAsync()
    {
        // Crea la base si no hay migraciones; util para bootstrap rapido
        await _context.Database.EnsureCreatedAsync();

        if (!await _context.Usuarios.AnyAsync())
        {
            var admin = new Usuario
            {
                Nombre = "Admin",
                Email = "admin@sfarma.com",
                Rol = "Admin"
            };
            admin.PasswordHash = _hasher.HashPassword(admin, "Admin123!");
            await _context.Usuarios.AddAsync(admin);
            await _context.SaveChangesAsync();
        }

        if (!await _context.Proveedores.AnyAsync())
        {
            var proveedores = new List<Proveedor>
            {
                new() { Nombre = "Salud Pharma", Contacto = "ventas@saludpharma.com", Direccion = "Av. Central 123" },
                new() { Nombre = "Botica Nova", Contacto = "contacto@boticanova.com", Direccion = "Jr. Salud 456" }
            };
            await _context.Proveedores.AddRangeAsync(proveedores);
            await _context.SaveChangesAsync();
        }

        var existingProducts = await _context.Productos.CountAsync();
        if (existingProducts < 8)
        {
            var productos = new List<Producto>
            {
                new() { Nombre = "Paracetamol 500mg", PrincipioActivo = "Paracetamol", Laboratorio = "Genfar", PrecioCompra = 0.50m, PrecioVenta = 1.00m, StockActual = 200, StockMinimo = 20, FechaVencimiento = DateTime.UtcNow.AddYears(1), Lote = "L-001", CodigoBarras = "750100000001" },
                new() { Nombre = "Ibuprofeno 400mg", PrincipioActivo = "Ibuprofeno", Laboratorio = "MK", PrecioCompra = 0.80m, PrecioVenta = 1.60m, StockActual = 150, StockMinimo = 15, FechaVencimiento = DateTime.UtcNow.AddYears(1).AddMonths(3), Lote = "L-002", CodigoBarras = "750100000002" },
                new() { Nombre = "Omeprazol 20mg", PrincipioActivo = "Omeprazol", Laboratorio = "Sandoz", PrecioCompra = 1.20m, PrecioVenta = 2.40m, StockActual = 120, StockMinimo = 10, FechaVencimiento = DateTime.UtcNow.AddYears(2), Lote = "L-003", CodigoBarras = "750100000003" },
                new() { Nombre = "Loratadina 10mg", PrincipioActivo = "Loratadina", Laboratorio = "Bayer", PrecioCompra = 0.70m, PrecioVenta = 1.50m, StockActual = 180, StockMinimo = 18, FechaVencimiento = DateTime.UtcNow.AddYears(1).AddMonths(6), Lote = "L-004", CodigoBarras = "750100000004" },
                new() { Nombre = "Vitamina C 1g", PrincipioActivo = "Acido ascorbico", Laboratorio = "Redoxon", PrecioCompra = 0.90m, PrecioVenta = 1.80m, StockActual = 100, StockMinimo = 10, FechaVencimiento = DateTime.UtcNow.AddYears(2), Lote = "L-005", CodigoBarras = "750100000005" },
                new() { Nombre = "Amoxicilina 500mg", PrincipioActivo = "Amoxicilina", Laboratorio = "Pfizer", PrecioCompra = 1.50m, PrecioVenta = 3.20m, StockActual = 90, StockMinimo = 10, FechaVencimiento = DateTime.UtcNow.AddYears(1).AddMonths(9), Lote = "L-006", CodigoBarras = "750100000006" },
                new() { Nombre = "Losartan 50mg", PrincipioActivo = "Losartan", Laboratorio = "Teva", PrecioCompra = 0.95m, PrecioVenta = 1.90m, StockActual = 140, StockMinimo = 14, FechaVencimiento = DateTime.UtcNow.AddYears(2), Lote = "L-007", CodigoBarras = "750100000007" },
                new() { Nombre = "Metformina 850mg", PrincipioActivo = "Metformina", Laboratorio = "Merck", PrecioCompra = 0.85m, PrecioVenta = 1.70m, StockActual = 160, StockMinimo = 16, FechaVencimiento = DateTime.UtcNow.AddYears(2), Lote = "L-008", CodigoBarras = "750100000008" }
            };
            await _context.Productos.AddRangeAsync(productos);
            await _context.SaveChangesAsync();
        }
    }
}
