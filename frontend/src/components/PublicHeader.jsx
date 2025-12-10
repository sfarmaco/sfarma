import { Link } from "react-router-dom";
import { useCart } from "../context/CartContext";
import { useState } from "react";

export default function PublicHeader() {
  const { items } = useCart();
  const count = items.reduce((sum, i) => sum + i.qty, 0);
  const [open, setOpen] = useState(false);

  return (
    <header className="bg-white shadow sticky top-0 z-30">
      <div className="max-w-6xl mx-auto px-4 sm:px-6 py-3 flex items-center justify-between">
        <Link to="/" className="flex items-center gap-2">
          <img src="/logo.png" alt="Sfarma" className="h-8 w-8 object-contain" />
          <span className="text-xl font-bold text-blue-700">Sfarma</span>
        </Link>
        <button
          className="md:hidden px-3 py-2 text-sm border rounded"
          onClick={() => setOpen((o) => !o)}
        >
          {open ? "Cerrar" : "Menú"}
        </button>
        <nav className="hidden md:flex items-center gap-4 text-sm font-medium">
          <a href="#proveedores" className="hover:text-blue-700">
            Proveedores
          </a>
          <a href="#mayoreo" className="hover:text-blue-700">
            Mayoreo
          </a>
          <a href="#erp" className="hover:text-blue-700">
            ERP
          </a>
          <Link to="/catalogo" className="hover:text-blue-700">
            Productos
          </Link>
          <Link to="/promos" className="hover:text-blue-700">
            Promos
          </Link>
          <Link to="/carrito" className="hover:text-blue-700">
            Carrito {count > 0 && <span className="ml-1 text-blue-700">({count})</span>}
          </Link>
          <Link to="/login" className="px-3 py-1 rounded border border-blue-600 text-blue-600 hover:bg-blue-50">
            Admin
          </Link>
        </nav>
      </div>
      {open && (
        <div className="md:hidden px-4 sm:px-6 pb-3">
          <div className="flex flex-col gap-2 text-sm font-medium">
            <a href="#proveedores" className="hover:text-blue-700" onClick={() => setOpen(false)}>
              Proveedores
            </a>
            <a href="#mayoreo" className="hover:text-blue-700" onClick={() => setOpen(false)}>
              Mayoreo
            </a>
            <a href="#erp" className="hover:text-blue-700" onClick={() => setOpen(false)}>
              ERP
            </a>
            <Link to="/catalogo" className="hover:text-blue-700" onClick={() => setOpen(false)}>
              Productos
            </Link>
            <Link to="/promos" className="hover:text-blue-700" onClick={() => setOpen(false)}>
              Promos
            </Link>
            <Link to="/carrito" className="hover:text-blue-700" onClick={() => setOpen(false)}>
              Carrito {count > 0 && <span className="ml-1 text-blue-700">({count})</span>}
            </Link>
            <Link
              to="/login"
              className="px-3 py-1 rounded border border-blue-600 text-blue-600 hover:bg-blue-50 inline-block"
              onClick={() => setOpen(false)}
            >
              Admin
            </Link>
          </div>
        </div>
      )}
    </header>
  );
}
