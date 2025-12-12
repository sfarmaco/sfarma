import { NavLink } from "react-router-dom";

export default function Sidebar() {
  const linkClass = ({ isActive }) =>
    `block px-3 py-2 rounded ${isActive ? "bg-blue-100 text-blue-700" : "hover:bg-gray-100"}`;
  return (
    <aside className="w-56 bg-white border-r p-4 space-y-2">
      <NavLink to="/admin" className={linkClass}>
        Dashboard
      </NavLink>
      <NavLink to="/admin/partners" className={linkClass}>
        Contactos
      </NavLink>
      <NavLink to="/admin/ventas" className={linkClass}>
        Ventas
      </NavLink>
      <NavLink to="/admin/productos" className={linkClass}>
        Productos
      </NavLink>
      <NavLink to="/admin/compras" className={linkClass}>
        Compras
      </NavLink>
      <NavLink to="/admin/inventario" className={linkClass}>
        Inventario
      </NavLink>
      <NavLink to="/admin/facturas" className={linkClass}>
        Facturas
      </NavLink>
      <NavLink to="/admin/proveedores" className={linkClass}>
        Proveedores
      </NavLink>
      <NavLink to="/admin/pos" className={linkClass}>
        POS
      </NavLink>
    </aside>
  );
}
