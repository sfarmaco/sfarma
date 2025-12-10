import { useEffect, useState } from "react";
import api from "../services/api";
import Header from "../components/Header";
import Sidebar from "../components/Sidebar";

export default function Pos() {
  const [productos, setProductos] = useState([]);
  const [carrito, setCarrito] = useState([]);

  useEffect(() => {
    api.get("/productos").then((r) => setProductos(r.data));
  }, []);

  const add = (prod) => {
    setCarrito((prev) => {
      const found = prev.find((p) => p.id === prod.id);
      if (found) return prev.map((p) => (p.id === prod.id ? { ...p, qty: p.qty + 1 } : p));
      return [...prev, { ...prod, qty: 1 }];
    });
  };

  const total = carrito.reduce((sum, item) => sum + item.qty * item.precioVenta, 0);

  const sell = async () => {
    const detalles = carrito.map((c) => ({
      productoId: c.id,
      cantidad: c.qty,
      precioUnitario: c.precioVenta,
    }));
    await api.post("/ventas", {
      fecha: new Date(),
      tipoVenta: "Contado",
      empleadoId: 1, // reemplazar con usuario logueado
      clienteId: null,
      detalles,
    });
    setCarrito([]);
  };

  return (
    <div className="min-h-screen flex">
      <Sidebar />
      <div className="flex-1">
        <Header />
        <div className="p-6 grid grid-cols-3 gap-4">
          <div className="col-span-2 bg-white p-4 rounded shadow">
            <h3 className="font-semibold mb-2">Productos</h3>
            <div className="grid grid-cols-3 gap-3">
              {productos.map((p) => (
                <button
                  key={p.id}
                  onClick={() => add(p)}
                  className="border p-3 rounded hover:shadow text-left"
                >
                  <p className="font-medium">{p.nombre}</p>
                  <p className="text-sm text-gray-500">{p.laboratorio}</p>
                  <p className="font-semibold">${p.precioVenta}</p>
                </button>
              ))}
            </div>
          </div>
          <div className="bg-white p-4 rounded shadow space-y-3">
            <h3 className="font-semibold">Carrito</h3>
            <ul className="divide-y">
              {carrito.map((item) => (
                <li key={item.id} className="py-2 flex justify-between">
                  <span>
                    {item.nombre} x {item.qty}
                  </span>
                  <span>${(item.qty * item.precioVenta).toFixed(2)}</span>
                </li>
              ))}
            </ul>
            <div className="font-semibold">Total: ${total.toFixed(2)}</div>
            <button onClick={sell} className="w-full bg-green-600 text-white py-2 rounded">
              Cobrar
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
