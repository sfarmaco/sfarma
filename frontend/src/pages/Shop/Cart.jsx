import { useState } from "react";
import PublicHeader from "../../components/PublicHeader";
import { useCart } from "../../context/CartContext";
import api from "../../services/api";

export default function Cart() {
  const { items, setQty, remove, clear } = useCart();
  const [status, setStatus] = useState("");
  const total = items.reduce((sum, i) => sum + i.qty * i.precioVenta, 0);

  const checkout = async () => {
    if (items.length === 0) return;
    setStatus("Procesando...");
    try {
      const detalles = items.map((i) => ({
        productoId: i.id,
        cantidad: i.qty,
        precioUnitario: i.precioVenta,
      }));
      await api.post("/store/checkout", {
        fecha: new Date(),
        tipoVenta: "Online",
        empleadoId: 1,
        clienteId: null,
        detalles,
      });
      clear();
      setStatus("Pedido registrado. ¡Gracias por tu compra!");
    } catch (e) {
      setStatus("No se pudo procesar el pedido. Intenta nuevamente.");
    }
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <PublicHeader />
      <main className="max-w-5xl mx-auto px-4 sm:px-6 py-8 sm:py-10 space-y-6">
        <h1 className="text-2xl font-bold">Carrito</h1>
        {items.length === 0 ? (
          <div className="bg-white p-6 rounded shadow text-gray-600">Tu carrito está vacío.</div>
        ) : (
          <div className="grid md:grid-cols-3 gap-4">
            <div className="md:col-span-2 bg-white rounded shadow divide-y">
              {items.map((item) => (
                <div key={item.id} className="p-4 flex items-center justify-between">
                  <div>
                    <div className="font-semibold">{item.nombre}</div>
                    <div className="text-sm text-gray-500">{item.laboratorio}</div>
                  </div>
                  <div className="flex items-center gap-2">
                    <input
                      type="number"
                      min="1"
                      className="w-16 border rounded px-2 py-1"
                      value={item.qty}
                      onChange={(e) => setQty(item.id, Number(e.target.value))}
                    />
                    <div className="font-semibold">${(item.precioVenta * item.qty).toFixed(2)}</div>
                    <button
                      onClick={() => remove(item.id)}
                      className="text-red-500 text-sm hover:underline"
                    >
                      Quitar
                    </button>
                  </div>
                </div>
              ))}
            </div>
            <div className="bg-white rounded shadow p-4 space-y-3">
              <div className="flex justify-between text-sm text-gray-700">
                <span>Productos</span>
                <span>{items.length}</span>
              </div>
              <div className="flex justify-between font-semibold text-lg">
                <span>Total</span>
                <span>${total.toFixed(2)}</span>
              </div>
              <button
                onClick={checkout}
                className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700"
              >
                Finalizar compra
              </button>
              {status && <div className="text-sm text-gray-600">{status}</div>}
            </div>
          </div>
        )}
      </main>
    </div>
  );
}
