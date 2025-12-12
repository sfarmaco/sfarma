import { useEffect, useState } from "react";
import Header from "../components/Header";
import Sidebar from "../components/Sidebar";
import api from "../services/api";

export default function Facturas() {
  const [partners, setPartners] = useState([]);
  const [productos, setProductos] = useState([]);
  const [facturas, setFacturas] = useState([]);
  const [form, setForm] = useState({ partnerId: "", tipo: "Customer", moneda: "USD", lineas: [] });
  const [line, setLine] = useState({ productoId: "", cantidad: 1, precio: 0, impuestos: 0 });

  const load = async () => {
    const [p, prod, inv] = await Promise.all([
      api.get("/partners"),
      api.get("/productos"),
      api.get("/invoices"),
    ]);
    setPartners(p.data);
    setProductos(prod.data);
    setFacturas(inv.data);
  };

  useEffect(() => {
    load();
  }, []);

  const addLine = () => {
    if (!line.productoId) return;
    const prod = productos.find((p) => p.id === Number(line.productoId));
    const precio = line.precio || prod?.precioVenta || 0;
    const nueva = {
      productoId: Number(line.productoId),
      cantidad: Number(line.cantidad),
      precioUnitario: Number(precio),
      impuestos: Number(line.impuestos),
    };
    setForm({ ...form, lineas: [...form.lineas, nueva] });
    setLine({ productoId: "", cantidad: 1, precio: 0, impuestos: 0 });
  };

  const createInvoice = async (e) => {
    e.preventDefault();
    if (!form.partnerId || form.lineas.length === 0) return;
    await api.post("/invoices", {
      partnerId: Number(form.partnerId),
      tipo: form.tipo,
      moneda: form.moneda,
      lineas: form.lineas,
    });
    setForm({ partnerId: "", tipo: "Customer", moneda: "USD", lineas: [] });
    load();
  };

  const changeState = async (id, state) => {
    await api.post(`/invoices/${id}/state/${state}`);
    load();
  };

  return (
    <div className="min-h-screen flex">
      <Sidebar />
      <div className="flex-1">
        <Header />
        <div className="p-6 grid lg:grid-cols-3 gap-4">
          <form onSubmit={createInvoice} className="bg-white p-4 rounded shadow space-y-2">
            <h3 className="font-semibold">Nueva factura</h3>
            <select
              className="w-full border p-2 rounded"
              value={form.partnerId}
              onChange={(e) => setForm({ ...form, partnerId: e.target.value })}
              required
            >
              <option value="">Seleccione partner</option>
              {partners.map((p) => (
                <option key={p.id} value={p.id}>
                  {p.nombre}
                </option>
              ))}
            </select>
            <select
              className="w-full border p-2 rounded"
              value={form.tipo}
              onChange={(e) => setForm({ ...form, tipo: e.target.value })}
            >
              <option value="Customer">Cliente</option>
              <option value="Vendor">Proveedor</option>
            </select>
            <div className="grid grid-cols-4 gap-2 items-end">
              <select
                className="col-span-2 border p-2 rounded"
                value={line.productoId}
                onChange={(e) => setLine({ ...line, productoId: e.target.value })}
              >
                <option value="">Producto</option>
                {productos.map((p) => (
                  <option key={p.id} value={p.id}>
                    {p.nombre}
                  </option>
                ))}
              </select>
              <input
                type="number"
                min="1"
                className="border p-2 rounded"
                value={line.cantidad}
                onChange={(e) => setLine({ ...line, cantidad: e.target.value })}
              />
              <button type="button" onClick={addLine} className="bg-blue-600 text-white px-3 py-2 rounded">
                + Línea
              </button>
            </div>
            <div className="grid grid-cols-3 gap-2">
              <input
                className="border p-2 rounded"
                placeholder="Precio"
                value={line.precio}
                onChange={(e) => setLine({ ...line, precio: e.target.value })}
              />
              <input
                className="border p-2 rounded"
                placeholder="Impuestos"
                value={line.impuestos}
                onChange={(e) => setLine({ ...line, impuestos: e.target.value })}
              />
              <input
                className="border p-2 rounded"
                value={form.moneda}
                onChange={(e) => setForm({ ...form, moneda: e.target.value })}
              />
            </div>
            <div className="bg-gray-50 p-2 rounded border">
              {form.lineas.length === 0 ? (
                <p className="text-sm text-gray-500">Sin líneas</p>
              ) : (
                <ul className="text-sm space-y-1">
                  {form.lineas.map((l, idx) => {
                    const prod = productos.find((p) => p.id === l.productoId);
                    return (
                      <li key={idx} className="flex justify-between">
                        <span>
                          {prod?.nombre || l.productoId} x {l.cantidad}
                        </span>
                        <span>${(l.cantidad * l.precioUnitario + l.impuestos).toFixed(2)}</span>
                      </li>
                    );
                  })}
                </ul>
              )}
            </div>
            <button className="w-full bg-green-600 text-white py-2 rounded">Crear factura</button>
          </form>

          <div className="lg:col-span-2 bg-white p-4 rounded shadow">
            <h3 className="font-semibold mb-2">Facturas</h3>
            <div className="overflow-auto">
              <table className="w-full text-sm">
                <thead>
                  <tr className="border-b text-left">
                    <th className="py-2">ID</th>
                    <th className="py-2">Partner</th>
                    <th className="py-2">Tipo</th>
                    <th className="py-2">Estado</th>
                    <th className="py-2">Total</th>
                    <th className="py-2">Acciones</th>
                  </tr>
                </thead>
                <tbody>
                  {facturas.map((f) => (
                    <tr key={f.id} className="border-b last:border-0">
                      <td className="py-2">{f.id}</td>
                      <td className="py-2">{f.partnerId}</td>
                      <td className="py-2">{f.tipo}</td>
                      <td className="py-2">{f.estado}</td>
                      <td className="py-2 font-semibold">${f.total}</td>
                      <td className="py-2 space-x-1">
                        {["Open", "Paid", "Cancelled"].map((st) => (
                          <button
                            key={st}
                            onClick={() => changeState(f.id, st)}
                            className="text-xs px-2 py-1 border rounded hover:bg-gray-50"
                          >
                            {st}
                          </button>
                        ))}
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
