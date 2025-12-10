import { useEffect, useState } from "react";
import api from "../services/api";
import Header from "../components/Header";
import Sidebar from "../components/Sidebar";

export default function Productos() {
  const [items, setItems] = useState([]);
  const [form, setForm] = useState({
    nombre: "",
    laboratorio: "",
    precioVenta: 0,
    codigoBarras: "",
  });

  const load = async () => {
    const { data } = await api.get("/productos");
    setItems(data);
  };

  useEffect(() => {
    load();
  }, []);

  const create = async (e) => {
    e.preventDefault();
    await api.post("/productos", {
      nombre: form.nombre,
      principioActivo: "",
      laboratorio: form.laboratorio,
      precioCompra: 0,
      precioVenta: Number(form.precioVenta),
      stockActual: 0,
      stockMinimo: 0,
      fechaVencimiento: new Date(),
      lote: "",
      codigoBarras: form.codigoBarras,
    });
    setForm({ nombre: "", laboratorio: "", precioVenta: 0, codigoBarras: "" });
    load();
  };

  return (
    <div className="min-h-screen flex">
      <Sidebar />
      <div className="flex-1">
        <Header />
        <div className="p-6 grid grid-cols-3 gap-4">
          <form onSubmit={create} className="space-y-2 bg-white p-4 rounded shadow">
            <h3 className="font-semibold">Nuevo producto</h3>
            <input
              className="w-full border p-2 rounded"
              placeholder="Nombre"
              value={form.nombre}
              onChange={(e) => setForm({ ...form, nombre: e.target.value })}
            />
            <input
              className="w-full border p-2 rounded"
              placeholder="Laboratorio"
              value={form.laboratorio}
              onChange={(e) => setForm({ ...form, laboratorio: e.target.value })}
            />
            <input
              className="w-full border p-2 rounded"
              placeholder="Precio venta"
              value={form.precioVenta}
              onChange={(e) => setForm({ ...form, precioVenta: e.target.value })}
            />
            <input
              className="w-full border p-2 rounded"
              placeholder="Código barras"
              value={form.codigoBarras}
              onChange={(e) =>
                setForm({ ...form, codigoBarras: e.target.value })
              }
            />
            <button className="w-full bg-blue-600 text-white py-2 rounded">
              Guardar
            </button>
          </form>
          <div className="col-span-2 bg-white p-4 rounded shadow">
            <h3 className="font-semibold mb-2">Listado</h3>
            <ul className="divide-y">
              {items.map((p) => (
                <li key={p.id} className="py-2 flex justify-between">
                  <div>
                    <p className="font-medium">{p.nombre}</p>
                    <p className="text-sm text-gray-500">{p.laboratorio}</p>
                  </div>
                  <span className="font-semibold">${p.precioVenta}</span>
                </li>
              ))}
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
}
