import { useEffect, useState } from "react";
import Header from "../components/Header";
import Sidebar from "../components/Sidebar";
import api from "../services/api";

export default function Partners() {
  const [items, setItems] = useState([]);
  const [form, setForm] = useState({
    nombre: "",
    tipo: "persona",
    email: "",
    telefono: "",
    direccion: "",
    ciudad: "",
    pais: "",
    esCliente: true,
    esProveedor: false,
  });

  const load = async () => {
    const { data } = await api.get("/partners");
    setItems(data);
  };

  useEffect(() => {
    load();
  }, []);

  const create = async (e) => {
    e.preventDefault();
    await api.post("/partners", form);
    setForm({
      nombre: "",
      tipo: "persona",
      email: "",
      telefono: "",
      direccion: "",
      ciudad: "",
      pais: "",
      esCliente: true,
      esProveedor: false,
    });
    load();
  };

  return (
    <div className="min-h-screen flex">
      <Sidebar />
      <div className="flex-1">
        <Header />
        <div className="p-6 grid md:grid-cols-3 gap-4">
          <form onSubmit={create} className="space-y-2 bg-white p-4 rounded shadow">
            <h3 className="font-semibold">Nuevo contacto</h3>
            <input
              className="w-full border p-2 rounded"
              placeholder="Nombre"
              value={form.nombre}
              onChange={(e) => setForm({ ...form, nombre: e.target.value })}
              required
            />
            <select
              className="w-full border p-2 rounded"
              value={form.tipo}
              onChange={(e) => setForm({ ...form, tipo: e.target.value })}
            >
              <option value="persona">Persona</option>
              <option value="empresa">Empresa</option>
            </select>
            <input
              className="w-full border p-2 rounded"
              placeholder="Email"
              value={form.email}
              onChange={(e) => setForm({ ...form, email: e.target.value })}
            />
            <input
              className="w-full border p-2 rounded"
              placeholder="Teléfono"
              value={form.telefono}
              onChange={(e) => setForm({ ...form, telefono: e.target.value })}
            />
            <input
              className="w-full border p-2 rounded"
              placeholder="Dirección"
              value={form.direccion}
              onChange={(e) => setForm({ ...form, direccion: e.target.value })}
            />
            <div className="grid grid-cols-2 gap-2">
              <input
                className="w-full border p-2 rounded"
                placeholder="Ciudad"
                value={form.ciudad}
                onChange={(e) => setForm({ ...form, ciudad: e.target.value })}
              />
              <input
                className="w-full border p-2 rounded"
                placeholder="País"
                value={form.pais}
                onChange={(e) => setForm({ ...form, pais: e.target.value })}
              />
            </div>
            <div className="flex items-center gap-3 text-sm">
              <label className="flex items-center gap-1">
                <input
                  type="checkbox"
                  checked={form.esCliente}
                  onChange={(e) => setForm({ ...form, esCliente: e.target.checked })}
                />
                Cliente
              </label>
              <label className="flex items-center gap-1">
                <input
                  type="checkbox"
                  checked={form.esProveedor}
                  onChange={(e) => setForm({ ...form, esProveedor: e.target.checked })}
                />
                Proveedor
              </label>
            </div>
            <button className="w-full bg-blue-600 text-white py-2 rounded">Guardar</button>
          </form>
          <div className="md:col-span-2 bg-white p-4 rounded shadow">
            <h3 className="font-semibold mb-2">Contactos</h3>
            <div className="overflow-auto">
              <table className="w-full text-sm">
                <thead>
                  <tr className="text-left border-b">
                    <th className="py-2">Nombre</th>
                    <th className="py-2">Email</th>
                    <th className="py-2">Teléfono</th>
                    <th className="py-2">Tipo</th>
                  </tr>
                </thead>
                <tbody>
                  {items.map((p) => (
                    <tr key={p.id} className="border-b last:border-0">
                      <td className="py-2">{p.nombre}</td>
                      <td className="py-2">{p.email}</td>
                      <td className="py-2">{p.telefono}</td>
                      <td className="py-2 text-xs text-gray-600">
                        {p.esCliente ? "Cliente " : ""}
                        {p.esProveedor ? "Proveedor" : ""}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
