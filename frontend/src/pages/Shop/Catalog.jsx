import { useEffect, useState } from "react";
import PublicHeader from "../../components/PublicHeader";
import api from "../../services/api";
import { useCart } from "../../context/CartContext";

export default function Catalog() {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(false);
  const { add } = useCart();

  useEffect(() => {
    const load = async () => {
      setLoading(true);
      try {
        const { data } = await api.get("/store/productos");
        setItems(data);
      } finally {
        setLoading(false);
      }
    };
    load();
  }, []);

  return (
    <div className="min-h-screen bg-gray-50">
      <PublicHeader />
      <main className="max-w-6xl mx-auto px-4 sm:px-6 py-8 sm:py-10">
        <div className="flex items-center justify-between mb-6 flex-wrap gap-2">
          <h1 className="text-2xl font-bold">Catálogo</h1>
          <div className="text-sm text-gray-600">{items.length} productos</div>
        </div>
        {loading ? (
          <div className="text-center text-gray-600">Cargando...</div>
        ) : (
          <div className="grid sm:grid-cols-2 md:grid-cols-3 gap-4">
            {items.map((p) => (
              <div key={p.id} className="bg-white rounded-xl p-4 shadow-sm border border-gray-100 flex flex-col">
                <div className="flex justify-between items-start">
                  <div>
                    <div className="text-xs text-gray-500 mb-1">{p.laboratorio}</div>
                    <h3 className="text-lg font-semibold">{p.nombre}</h3>
                  </div>
                  <span className="text-xs px-2 py-1 bg-amber-100 text-amber-800 rounded-full">Farmacia</span>
                </div>
                <div className="text-sm text-gray-500">Stock: {p.stockActual}</div>
                <div className="text-sm text-gray-500">Principio: {p.principioActivo}</div>
                <div className="text-xl font-bold mt-2">${p.precioVenta}</div>
                <button
                  onClick={() => add(p)}
                  className="mt-3 w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
                >
                  Agregar al carrito
                </button>
              </div>
            ))}
          </div>
        )}
      </main>
    </div>
  );
}
