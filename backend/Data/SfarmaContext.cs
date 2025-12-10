using Microsoft.EntityFrameworkCore;
using Sfarma.Api.Models;

namespace Sfarma.Api.Data;

public class SfarmaContext : DbContext
{
    public SfarmaContext(DbContextOptions<SfarmaContext> options) : base(options) { }

    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<DetalleVenta> DetallesVenta => Set<DetalleVenta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("usuarios");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.Nombre).HasColumnName("nombre");
            e.Property(p => p.Email).HasColumnName("email");
            e.Property(p => p.PasswordHash).HasColumnName("password_hash");
            e.Property(p => p.Rol).HasColumnName("rol").HasMaxLength(30);
            e.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Producto>(e =>
        {
            e.ToTable("productos");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.Nombre).HasColumnName("nombre");
            e.Property(p => p.PrincipioActivo).HasColumnName("principio_activo");
            e.Property(p => p.Laboratorio).HasColumnName("laboratorio");
            e.Property(p => p.ProveedorId).HasColumnName("proveedor_id");
            e.Property(p => p.PrecioCompra).HasColumnName("precio_compra");
            e.Property(p => p.PrecioVenta).HasColumnName("precio_venta");
            e.Property(p => p.StockActual).HasColumnName("stock_actual");
            e.Property(p => p.StockMinimo).HasColumnName("stock_minimo");
            e.Property(p => p.FechaVencimiento).HasColumnName("fecha_vencimiento");
            e.Property(p => p.Lote).HasColumnName("lote");
            e.Property(p => p.CodigoBarras).HasColumnName("codigo_barras");
            e.Property(p => p.PrecioCompra).HasColumnType("decimal(18,2)");
            e.Property(p => p.PrecioVenta).HasColumnType("decimal(18,2)");
            e.HasIndex(p => p.CodigoBarras).IsUnique();
            e.HasOne(p => p.Proveedor).WithMany(r => r.Productos).HasForeignKey(p => p.ProveedorId).HasConstraintName("fk_producto_proveedor");
        });

        modelBuilder.Entity<Proveedor>(e =>
        {
            e.ToTable("proveedores");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.Nombre).HasColumnName("nombre");
            e.Property(p => p.Contacto).HasColumnName("contacto");
            e.Property(p => p.Direccion).HasColumnName("direccion");
        });

        modelBuilder.Entity<Venta>(e =>
        {
            e.ToTable("ventas");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.Fecha).HasColumnName("fecha");
            e.Property(p => p.Total).HasColumnName("total");
            e.Property(p => p.TipoVenta).HasColumnName("tipo_venta");
            e.Property(p => p.EmpleadoId).HasColumnName("empleado_id");
            e.Property(p => p.ClienteId).HasColumnName("cliente_id");
            e.Property(v => v.Total).HasColumnType("decimal(18,2)");
            e.HasMany(v => v.Detalles).WithOne(d => d.Venta!).HasForeignKey(d => d.VentaId).HasConstraintName("fk_detalle_venta");
            e.HasOne(v => v.Empleado).WithMany(u => u.Ventas).HasForeignKey(v => v.EmpleadoId).HasConstraintName("fk_venta_empleado");
        });

        modelBuilder.Entity<DetalleVenta>(e =>
        {
            e.ToTable("detalle_venta");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.VentaId).HasColumnName("venta_id");
            e.Property(p => p.ProductoId).HasColumnName("producto_id");
            e.Property(p => p.Cantidad).HasColumnName("cantidad");
            e.Property(p => p.PrecioUnitario).HasColumnName("precio_unitario");
            e.Property(d => d.PrecioUnitario).HasColumnType("decimal(18,2)");
        });
    }
}
