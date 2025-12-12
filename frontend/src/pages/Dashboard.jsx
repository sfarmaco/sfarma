import { useEffect, useState } from "react";
import Header from "../components/Header";
import Sidebar from "../components/Sidebar";
import api from "../services/api";

export default function Dashboard() {
  const [data, setData] = useState(null);
  const load = async () => {
    try {
      const { data } = await api.get("/dashboard");
      setData(data);
    } catch {
      setData(null);
    }
  };

  useEffect(() => {
    load();
  }, []);

  return (
    <div className="min-h-screen flex">
      <Sidebar />
      <div className="flex-1">
        <Header />
        <div className="p-6 grid md:grid-cols-3 gap-4">
          {data ? (
            <>
              <Kpi title="Productos" value={data.productos} />
              <Kpi title="Contactos" value={data.partners} />
              <Kpi title="OV totales" value={data.saleOrders} secondary={`$${data.saleOrdersTotal}`} />
              <Kpi title="OC totales" value={data.purchaseOrders} secondary={`$${data.purchaseOrdersTotal}`} />
              <Kpi title="Facturas" value={data.invoices} secondary={`$${data.invoicesTotal}`} />
            </>
          ) : (
            <div className="col-span-3 text-sm text-gray-600">Sin datos</div>
          )}
        </div>
      </div>
    </div>
  );
}

function Kpi({ title, value, secondary }) {
  return (
    <div className="bg-white rounded shadow p-4 border border-gray-100">
      <div className="text-sm text-gray-500">{title}</div>
      <div className="text-2xl font-bold">{value}</div>
      {secondary && <div className="text-sm text-gray-600">{secondary}</div>}
    </div>
  );
}
