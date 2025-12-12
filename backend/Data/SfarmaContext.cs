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
    public DbSet<Partner> Partners => Set<Partner>();
    public DbSet<SaleOrder> SaleOrders => Set<SaleOrder>();
    public DbSet<SaleOrderLine> SaleOrderLines => Set<SaleOrderLine>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderLine> PurchaseOrderLines => Set<PurchaseOrderLine>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Picking> Pickings => Set<Picking>();
    public DbSet<Move> Moves => Set<Move>();
    public DbSet<Lot> Lots => Set<Lot>();

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

        modelBuilder.Entity<Partner>(e =>
        {
            e.ToTable("partners");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.Nombre).HasColumnName("nombre");
            e.Property(p => p.Tipo).HasColumnName("tipo");
            e.Property(p => p.Email).HasColumnName("email");
            e.Property(p => p.Telefono).HasColumnName("telefono");
            e.Property(p => p.Direccion).HasColumnName("direccion");
            e.Property(p => p.Ciudad).HasColumnName("ciudad");
            e.Property(p => p.Pais).HasColumnName("pais");
            e.Property(p => p.EsCliente).HasColumnName("es_cliente");
            e.Property(p => p.EsProveedor).HasColumnName("es_proveedor");
        });

        modelBuilder.Entity<SaleOrder>(e =>
        {
            e.ToTable("sale_orders");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.PartnerId).HasColumnName("partner_id");
            e.Property(p => p.Estado).HasColumnName("estado");
            e.Property(p => p.Fecha).HasColumnName("fecha");
            e.Property(p => p.FechaEntrega).HasColumnName("fecha_entrega");
            e.Property(p => p.Moneda).HasColumnName("moneda");
            e.Property(p => p.Total).HasColumnName("total").HasColumnType("decimal(18,2)");
            e.HasOne(p => p.Partner).WithMany(c => c.SaleOrders).HasForeignKey(p => p.PartnerId).HasConstraintName("fk_saleorder_partner");
        });

        modelBuilder.Entity<SaleOrderLine>(e =>
        {
            e.ToTable("sale_order_lines");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.SaleOrderId).HasColumnName("sale_order_id");
            e.Property(p => p.ProductoId).HasColumnName("producto_id");
            e.Property(p => p.Cantidad).HasColumnName("cantidad");
            e.Property(p => p.PrecioUnitario).HasColumnName("precio_unitario").HasColumnType("decimal(18,2)");
            e.Property(p => p.Impuestos).HasColumnName("impuestos").HasColumnType("decimal(18,2)");
            e.Property(p => p.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(18,2)");
            e.HasOne(p => p.SaleOrder).WithMany(o => o.Lineas).HasForeignKey(p => p.SaleOrderId).HasConstraintName("fk_saleorderline_order");
            e.HasOne(p => p.Producto).WithMany().HasForeignKey(p => p.ProductoId).HasConstraintName("fk_saleorderline_producto");
        });

        modelBuilder.Entity<PurchaseOrder>(e =>
        {
            e.ToTable("purchase_orders");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.ProveedorId).HasColumnName("proveedor_id");
            e.Property(p => p.Estado).HasColumnName("estado");
            e.Property(p => p.Fecha).HasColumnName("fecha");
            e.Property(p => p.FechaEntrega).HasColumnName("fecha_entrega");
            e.Property(p => p.Moneda).HasColumnName("moneda");
            e.Property(p => p.Total).HasColumnName("total").HasColumnType("decimal(18,2)");
            e.HasOne(p => p.Proveedor).WithMany().HasForeignKey(p => p.ProveedorId).HasConstraintName("fk_purchase_proveedor");
        });

        modelBuilder.Entity<PurchaseOrderLine>(e =>
        {
            e.ToTable("purchase_order_lines");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.PurchaseOrderId).HasColumnName("purchase_order_id");
            e.Property(p => p.ProductoId).HasColumnName("producto_id");
            e.Property(p => p.Cantidad).HasColumnName("cantidad");
            e.Property(p => p.PrecioUnitario).HasColumnName("precio_unitario").HasColumnType("decimal(18,2)");
            e.Property(p => p.Impuestos).HasColumnName("impuestos").HasColumnType("decimal(18,2)");
            e.Property(p => p.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(18,2)");
            e.HasOne(p => p.PurchaseOrder).WithMany(o => o.Lineas).HasForeignKey(p => p.PurchaseOrderId).HasConstraintName("fk_polines_order");
            e.HasOne(p => p.Producto).WithMany().HasForeignKey(p => p.ProductoId).HasConstraintName("fk_polines_producto");
        });

        modelBuilder.Entity<Invoice>(e =>
        {
            e.ToTable("invoices");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.PartnerId).HasColumnName("partner_id");
            e.Property(p => p.Tipo).HasColumnName("tipo");
            e.Property(p => p.Estado).HasColumnName("estado");
            e.Property(p => p.Fecha).HasColumnName("fecha");
            e.Property(p => p.Moneda).HasColumnName("moneda");
            e.Property(p => p.Total).HasColumnName("total").HasColumnType("decimal(18,2)");
            e.HasOne(p => p.Partner).WithMany().HasForeignKey(p => p.PartnerId).HasConstraintName("fk_invoice_partner");
        });

        modelBuilder.Entity<InvoiceLine>(e =>
        {
            e.ToTable("invoice_lines");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.InvoiceId).HasColumnName("invoice_id");
            e.Property(p => p.ProductoId).HasColumnName("producto_id");
            e.Property(p => p.Cantidad).HasColumnName("cantidad");
            e.Property(p => p.PrecioUnitario).HasColumnName("precio_unitario").HasColumnType("decimal(18,2)");
            e.Property(p => p.Impuestos).HasColumnName("impuestos").HasColumnType("decimal(18,2)");
            e.Property(p => p.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(18,2)");
            e.HasOne(p => p.Invoice).WithMany(o => o.Lineas).HasForeignKey(p => p.InvoiceId).HasConstraintName("fk_invoiceline_invoice");
            e.HasOne(p => p.Producto).WithMany().HasForeignKey(p => p.ProductoId).HasConstraintName("fk_invoiceline_producto");
        });

        modelBuilder.Entity<Location>(e =>
        {
            e.ToTable("locations");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.Nombre).HasColumnName("nombre");
            e.Property(p => p.Tipo).HasColumnName("tipo");
            e.Property(p => p.ParentId).HasColumnName("parent_id");
        });

        modelBuilder.Entity<Picking>(e =>
        {
            e.ToTable("pickings");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.Tipo).HasColumnName("tipo");
            e.Property(p => p.Estado).HasColumnName("estado");
            e.Property(p => p.OrigenId).HasColumnName("origen_id");
            e.Property(p => p.DestinoId).HasColumnName("destino_id");
            e.HasOne(p => p.Origen).WithMany().HasForeignKey(p => p.OrigenId).HasConstraintName("fk_pickings_origen");
            e.HasOne(p => p.Destino).WithMany().HasForeignKey(p => p.DestinoId).HasConstraintName("fk_pickings_destino");
        });

        modelBuilder.Entity<Move>(e =>
        {
            e.ToTable("moves");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.PickingId).HasColumnName("picking_id");
            e.Property(p => p.ProductoId).HasColumnName("producto_id");
            e.Property(p => p.Cantidad).HasColumnName("cantidad");
            e.Property(p => p.Estado).HasColumnName("estado");
            e.Property(p => p.LoteId).HasColumnName("lote_id");
            e.HasOne(p => p.Picking).WithMany(o => o.Movimientos).HasForeignKey(p => p.PickingId).HasConstraintName("fk_move_picking");
            e.HasOne(p => p.Producto).WithMany().HasForeignKey(p => p.ProductoId).HasConstraintName("fk_move_producto");
        });

        modelBuilder.Entity<Lot>(e =>
        {
            e.ToTable("lots");
            e.Property(p => p.Id).HasColumnName("id");
            e.Property(p => p.ProductoId).HasColumnName("producto_id");
            e.Property(p => p.LoteCodigo).HasColumnName("lote_codigo");
            e.Property(p => p.FechaVencimiento).HasColumnName("fecha_vencimiento");
            e.Property(p => p.CantidadDisponible).HasColumnName("cantidad_disponible").HasColumnType("decimal(18,2)");
            e.HasOne(p => p.Producto).WithMany().HasForeignKey(p => p.ProductoId).HasConstraintName("fk_lot_producto");
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
