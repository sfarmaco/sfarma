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

        // Crear tablas faltantes para compatibilidad en entornos donde EnsureCreated no agrega nuevas tablas
        await _context.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE IF NOT EXISTS partners (
                id SERIAL PRIMARY KEY,
                nombre TEXT NOT NULL,
                tipo TEXT,
                email TEXT,
                telefono TEXT,
                direccion TEXT,
                ciudad TEXT,
                pais TEXT,
                es_cliente BOOLEAN DEFAULT TRUE,
                es_proveedor BOOLEAN DEFAULT FALSE
            );
            CREATE TABLE IF NOT EXISTS sale_orders (
                id SERIAL PRIMARY KEY,
                partner_id INT NOT NULL REFERENCES partners(id),
                estado INT NOT NULL,
                fecha TIMESTAMP NOT NULL,
                fecha_entrega TIMESTAMP NULL,
                moneda TEXT NOT NULL,
                total NUMERIC(18,2) NOT NULL DEFAULT 0
            );
            CREATE TABLE IF NOT EXISTS sale_order_lines (
                id SERIAL PRIMARY KEY,
                sale_order_id INT NOT NULL REFERENCES sale_orders(id) ON DELETE CASCADE,
                producto_id INT NOT NULL REFERENCES productos(id),
                cantidad INT NOT NULL,
                precio_unitario NUMERIC(18,2) NOT NULL,
                impuestos NUMERIC(18,2) NOT NULL DEFAULT 0,
                subtotal NUMERIC(18,2) NOT NULL
            );
            CREATE TABLE IF NOT EXISTS purchase_orders (
                id SERIAL PRIMARY KEY,
                proveedor_id INT NOT NULL REFERENCES proveedores(id),
                estado INT NOT NULL,
                fecha TIMESTAMP NOT NULL,
                fecha_entrega TIMESTAMP NULL,
                moneda TEXT NOT NULL,
                total NUMERIC(18,2) NOT NULL DEFAULT 0
            );
            CREATE TABLE IF NOT EXISTS purchase_order_lines (
                id SERIAL PRIMARY KEY,
                purchase_order_id INT NOT NULL REFERENCES purchase_orders(id) ON DELETE CASCADE,
                producto_id INT NOT NULL REFERENCES productos(id),
                cantidad INT NOT NULL,
                precio_unitario NUMERIC(18,2) NOT NULL,
                impuestos NUMERIC(18,2) NOT NULL DEFAULT 0,
                subtotal NUMERIC(18,2) NOT NULL
            );
            CREATE TABLE IF NOT EXISTS invoices (
                id SERIAL PRIMARY KEY,
                partner_id INT NOT NULL REFERENCES partners(id),
                tipo INT NOT NULL,
                estado INT NOT NULL,
                fecha TIMESTAMP NOT NULL,
                moneda TEXT NOT NULL,
                total NUMERIC(18,2) NOT NULL DEFAULT 0
            );
            CREATE TABLE IF NOT EXISTS invoice_lines (
                id SERIAL PRIMARY KEY,
                invoice_id INT NOT NULL REFERENCES invoices(id) ON DELETE CASCADE,
                producto_id INT NOT NULL REFERENCES productos(id),
                cantidad INT NOT NULL,
                precio_unitario NUMERIC(18,2) NOT NULL,
                impuestos NUMERIC(18,2) NOT NULL DEFAULT 0,
                subtotal NUMERIC(18,2) NOT NULL
            );
            CREATE TABLE IF NOT EXISTS locations (
                id SERIAL PRIMARY KEY,
                nombre TEXT NOT NULL,
                tipo INT NOT NULL,
                parent_id INT NULL
            );
            CREATE TABLE IF NOT EXISTS pickings (
                id SERIAL PRIMARY KEY,
                tipo INT NOT NULL,
                estado INT NOT NULL,
                origen_id INT NULL REFERENCES locations(id),
                destino_id INT NULL REFERENCES locations(id)
            );
            CREATE TABLE IF NOT EXISTS moves (
                id SERIAL PRIMARY KEY,
                picking_id INT NOT NULL REFERENCES pickings(id) ON DELETE CASCADE,
                producto_id INT NOT NULL REFERENCES productos(id),
                cantidad NUMERIC(18,2) NOT NULL,
                estado INT NOT NULL,
                lote_id INT NULL
            );
            CREATE TABLE IF NOT EXISTS lots (
                id SERIAL PRIMARY KEY,
                producto_id INT NOT NULL REFERENCES productos(id),
                lote_codigo TEXT NOT NULL,
                fecha_vencimiento DATE NULL,
                cantidad_disponible NUMERIC(18,2) NOT NULL DEFAULT 0
            );
        ");

        if (!await _context.Usuarios.AnyAsync())
        {
            var usuarios = new List<Usuario>
            {
                new() { Nombre = "Admin", Email = "admin@sfarma.com", Rol = "Admin", PasswordHash = string.Empty },
                new() { Nombre = "Ventas", Email = "ventas@sfarma.com", Rol = "Sales", PasswordHash = string.Empty },
                new() { Nombre = "Compras", Email = "compras@sfarma.com", Rol = "Purchase", PasswordHash = string.Empty },
                new() { Nombre = "Inventario", Email = "inventario@sfarma.com", Rol = "Inventory", PasswordHash = string.Empty },
                new() { Nombre = "Contabilidad", Email = "contabilidad@sfarma.com", Rol = "Accounting", PasswordHash = string.Empty }
            };

            foreach (var u in usuarios)
            {
                var pwd = u.Rol switch
                {
                    "Sales" => "Sales123!",
                    "Purchase" => "Purchase123!",
                    "Inventory" => "Inventory123!",
                    "Accounting" => "Accounting123!",
                    _ => "Admin123!"
                };
                u.PasswordHash = _hasher.HashPassword(u, pwd);
            }

            await _context.Usuarios.AddRangeAsync(usuarios);
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

        if (!await _context.Partners.AnyAsync())
        {
            var partner = new Partner
            {
                Nombre = "Cliente Demo",
                Tipo = "persona",
                Email = "cliente@sfarma.com",
                Telefono = "999-999",
                EsCliente = true,
                EsProveedor = false,
                Ciudad = "Lima",
                Pais = "PE"
            };
            await _context.Partners.AddAsync(partner);
            await _context.SaveChangesAsync();
        }

        // Ubicaciones base
        if (!await _context.Locations.AnyAsync())
        {
            var stock = new Location { Nombre = "WH/Stock", Tipo = LocationType.Internal };
            var customer = new Location { Nombre = "Customer", Tipo = LocationType.Customer };
            var supplier = new Location { Nombre = "Supplier", Tipo = LocationType.Supplier };
            await _context.Locations.AddRangeAsync(stock, customer, supplier);
            await _context.SaveChangesAsync();
        }
    }
}
