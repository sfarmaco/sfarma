import { useEffect, useState } from "react";
import api from "../services/api";
import Header from "../components/Header";
import Sidebar from "../components/Sidebar";

export default function Proveedores() {
  const [items, setItems] = useState([]);
  const [form, setForm] = useState({ nombre: "", contacto: "", direccion: "" });

  const load = async () => {
    const { data } = await api.get("/proveedores");
    setItems(data);
  };

  useEffect(() => {
    load();
  }, []);

  const create = async (e) => {
    e.preventDefault();
    await api.post("/proveedores", form);
    setForm({ nombre: "", contacto: "", direccion: "" });
    load();
  };

  return (
    <div className="min-h-screen flex">
      <Sidebar />
      <div className="flex-1">
        <Header />
        <div className="p-6 grid grid-cols-2 gap-4">
          <form onSubmit={create} className="space-y-2 bg-white p-4 rounded shadow">
            <h3 className="font-semibold">Nuevo proveedor</h3>
            <input
              className="w-full border p-2 rounded"
              placeholder="Nombre"
              value={form.nombre}
              onChange={(e) => setForm({ ...form, nombre: e.target.value })}
            />
            <input
              className="w-full border p-2 rounded"
              placeholder="Contacto"
              value={form.contacto}
              onChange={(e) => setForm({ ...form, contacto: e.target.value })}
            />
            <input
              className="w-full border p-2 rounded"
              placeholder="Dirección"
              value={form.direccion}
              onChange={(e) => setForm({ ...form, direccion: e.target.value })}
            />
            <button className="w-full bg-blue-600 text-white py-2 rounded">
              Guardar
            </button>
          </form>
          <div className="bg-white p-4 rounded shadow">
            <h3 className="font-semibold mb-2">Listado</h3>
            <ul className="divide-y">
              {items.map((p) => (
                <li key={p.id} className="py-2">
                  <p className="font-medium">{p.nombre}</p>
                  <p className="text-sm text-gray-500">
                    {p.contacto} · {p.direccion}
                  </p>
                </li>
              ))}
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
}
