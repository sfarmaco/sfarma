import { useEffect, useState } from "react";
import Header from "../components/Header";
import Sidebar from "../components/Sidebar";
import api from "../services/api";

export default function Inventario() {
  const [locations, setLocations] = useState([]);
  const [pickings, setPickings] = useState([]);
  const [productos, setProductos] = useState([]);
  const [locForm, setLocForm] = useState({ nombre: "", tipo: "Internal", parentId: null });
  const [pickForm, setPickForm] = useState({ tipo: "Outgoing", origenId: null, destinoId: null, movimientos: [] });
  const [move, setMove] = useState({ productoId: "", cantidad: 1, loteId: null });

  const load = async () => {
    const [locs, picks, prods] = await Promise.all([
      api.get("/inventory/locations"),
      api.get("/inventory/pickings"),
      api.get("/productos"),
    ]);
    setLocations(locs.data);
    setPickings(picks.data);
    setProductos(prods.data);
  };

  useEffect(() => {
    load();
  }, []);

  const addLocation = async (e) => {
    e.preventDefault();
    await api.post("/inventory/locations", {
      nombre: locForm.nombre,
      tipo: locForm.tipo,
      parentId: locForm.parentId ? Number(locForm.parentId) : null,
    });
    setLocForm({ nombre: "", tipo: "Internal", parentId: null });
    load();
  };

  const addMove = () => {
    if (!move.productoId) return;
    const nueva = {
      productoId: Number(move.productoId),
      cantidad: Number(move.cantidad),
      loteId: move.loteId ? Number(move.loteId) : null,
    };
    setPickForm({ ...pickForm, movimientos: [...pickForm.movimientos, nueva] });
    setMove({ productoId: "", cantidad: 1, loteId: null });
  };

  const createPicking = async (e) => {
    e.preventDefault();
    if (pickForm.movimientos.length === 0) return;
    await api.post("/inventory/pickings", {
      tipo: pickForm.tipo,
      origenId: pickForm.origenId ? Number(pickForm.origenId) : null,
      destinoId: pickForm.destinoId ? Number(pickForm.destinoId) : null,
      movimientos: pickForm.movimientos,
    });
    setPickForm({ tipo: "Outgoing", origenId: null, destinoId: null, movimientos: [] });
    load();
  };

  const changeState = async (id, state) => {
    await api.post(`/inventory/pickings/${id}/state/${state}`);
    load();
  };

  return (
    <div className="min-h-screen flex">
      <Sidebar />
      <div className="flex-1">
        <Header />
        <div className="p-6 grid lg:grid-cols-3 gap-4">
          <form onSubmit={addLocation} className="bg-white p-4 rounded shadow space-y-2">
            <h3 className="font-semibold">Nueva ubicación</h3>
            <input
              className="w-full border p-2 rounded"
              placeholder="Nombre"
              value={locForm.nombre}
              onChange={(e) => setLocForm({ ...locForm, nombre: e.target.value })}
              required
            />
            <select
              className="w-full border p-2 rounded"
              value={locForm.tipo}
              onChange={(e) => setLocForm({ ...locForm, tipo: e.target.value })}
            >
              <option value="Internal">Interna</option>
              <option value="Customer">Cliente</option>
              <option value="Supplier">Proveedor</option>
              <option value="Transit">Tránsito</option>
            </select>
            <select
              className="w-full border p-2 rounded"
              value={locForm.parentId || ""}
              onChange={(e) => setLocForm({ ...locForm, parentId: e.target.value || null })}
            >
              <option value="">Sin padre</option>
              {locations.map((l) => (
                <option key={l.id} value={l.id}>
                  {l.nombre}
                </option>
              ))}
            </select>
            <button className="w-full bg-blue-600 text-white py-2 rounded">Guardar ubicación</button>
          </form>

          <form onSubmit={createPicking} className="bg-white p-4 rounded shadow space-y-2">
            <h3 className="font-semibold">Nuevo picking</h3>
            <select
              className="w-full border p-2 rounded"
              value={pickForm.tipo}
              onChange={(e) => setPickForm({ ...pickForm, tipo: e.target.value })}
            >
              <option value="Outgoing">Salida</option>
              <option value="Incoming">Entrada</option>
              <option value="Internal">Interno</option>
            </select>
            <select
              className="w-full border p-2 rounded"
              value={pickForm.origenId || ""}
              onChange={(e) => setPickForm({ ...pickForm, origenId: e.target.value || null })}
            >
              <option value="">Origen</option>
              {locations.map((l) => (
                <option key={l.id} value={l.id}>
                  {l.nombre}
                </option>
              ))}
            </select>
            <select
              className="w-full border p-2 rounded"
              value={pickForm.destinoId || ""}
              onChange={(e) => setPickForm({ ...pickForm, destinoId: e.target.value || null })}
            >
              <option value="">Destino</option>
              {locations.map((l) => (
                <option key={l.id} value={l.id}>
                  {l.nombre}
                </option>
              ))}
            </select>
            <div className="grid grid-cols-4 gap-2 items-end">
              <select
                className="col-span-2 border p-2 rounded"
                value={move.productoId}
                onChange={(e) => setMove({ ...move, productoId: e.target.value })}
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
                min="0.01"
                step="0.01"
                className="border p-2 rounded"
                value={move.cantidad}
                onChange={(e) => setMove({ ...move, cantidad: e.target.value })}
              />
              <button type="button" onClick={addMove} className="bg-blue-600 text-white px-3 py-2 rounded">
                + Move
              </button>
            </div>
            <div className="bg-gray-50 p-2 rounded border">
              {pickForm.movimientos.length === 0 ? (
                <p className="text-sm text-gray-500">Sin movimientos</p>
              ) : (
                <ul className="text-sm space-y-1">
                  {pickForm.movimientos.map((m, idx) => {
                    const prod = productos.find((p) => p.id === m.productoId);
                    return (
                      <li key={idx} className="flex justify-between">
                        <span>
                          {prod?.nombre || m.productoId} x {m.cantidad}
                        </span>
                      </li>
                    );
                  })}
                </ul>
              )}
            </div>
            <button className="w-full bg-green-600 text-white py-2 rounded">Crear picking</button>
          </form>

          <div className="lg:col-span-1 bg-white p-4 rounded shadow">
            <h3 className="font-semibold mb-2">Pickings</h3>
            <div className="space-y-2 max-h-[500px] overflow-auto">
              {pickings.map((p) => (
                <div key={p.id} className="border rounded p-2">
                  <div className="flex justify-between text-sm">
                    <span>#{p.id}</span>
                    <span>{p.estado}</span>
                  </div>
                  <div className="text-xs text-gray-600">Tipo: {p.tipo}</div>
                  <div className="text-xs text-gray-600">Moves: {p.movimientos.length}</div>
                  <div className="flex gap-1 mt-2 flex-wrap">
                    {["Reserved", "Done"].map((st) => (
                      <button
                        key={st}
                        onClick={() => changeState(p.id, st)}
                        className="text-xs px-2 py-1 border rounded hover:bg-gray-50"
                      >
                        {st}
                      </button>
                    ))}
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
