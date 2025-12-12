import { useEffect, useMemo, useState } from "react";
import Header from "../components/Header";
import Sidebar from "../components/Sidebar";
import api from "../services/api";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  ArcElement,
  Tooltip,
  Legend,
} from "chart.js";
import { Line, Bar, Doughnut } from "react-chartjs-2";

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, BarElement, ArcElement, Tooltip, Legend);

export default function Dashboard() {
  const [summary, setSummary] = useState(null);
  const [sales, setSales] = useState([]);
  const [purchases, setPurchases] = useState([]);
  const [invoices, setInvoices] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const load = async () => {
      setLoading(true);
      try {
        const [kpiRes, soRes, poRes, invRes] = await Promise.all([
          api.get("/dashboard"),
          api.get("/saleorders"),
          api.get("/purchaseorders"),
          api.get("/invoices"),
        ]);
        setSummary(kpiRes.data);
        setSales(soRes.data || []);
        setPurchases(poRes.data || []);
        setInvoices(invRes.data || []);
      } catch (e) {
        setSummary(null);
      } finally {
        setLoading(false);
      }
    };
    load();
  }, []);

  const monthLabels = useMemo(() => buildLabels(sales, purchases), [sales, purchases]);
  const salesSeries = useMemo(() => groupByMonthTotal(sales, monthLabels), [sales, monthLabels]);
  const purchaseSeries = useMemo(() => groupByMonthTotal(purchases, monthLabels), [purchases, monthLabels]);
  const invoiceSeries = useMemo(() => groupInvoices(invoices), [invoices]);

  const cards = [
    {
      title: "Ingresos ventas",
      value: money(summary?.saleOrdersTotal),
      secondary: `${summary?.saleOrders || 0} OV`,
    },
    {
      title: "Compras",
      value: money(summary?.purchaseOrdersTotal),
      secondary: `${summary?.purchaseOrders || 0} OC`,
    },
    {
      title: "Ticket promedio",
      value: money(calcAvg(sales)),
      secondary: "OV promedio",
    },
    {
      title: "Facturas abiertas",
      value: invoiceSeries.open,
      secondary: money(invoiceSeries.openTotal),
    },
    {
      title: "Productos",
      value: summary?.productos ?? 0,
      secondary: "Catálogo activo",
    },
    {
      title: "Contactos",
      value: summary?.partners ?? 0,
      secondary: "Clientes/Proveedores",
    },
  ];

  return (
    <div className="min-h-screen flex">
      <Sidebar />
      <div className="flex-1">
        <Header />
        <div className="p-6 space-y-6">
          <div className="grid md:grid-cols-3 gap-4">
            {loading && !summary && <div className="col-span-3 text-sm text-gray-600">Cargando KPIs...</div>}
            {!loading &&
              cards.map((c) => (
                <Kpi key={c.title} title={c.title} value={c.value} secondary={c.secondary} />
              ))}
          </div>

          <div className="grid lg:grid-cols-2 gap-6">
            <ChartCard title="Órdenes de venta por mes">
              {salesSeries.data.some((v) => v > 0) ? (
                <Line
                  data={{
                    labels: monthLabels,
                    datasets: [
                      {
                        label: "Ventas (PEN)",
                        data: salesSeries.data,
                        borderColor: "#2563eb",
                        backgroundColor: "rgba(37, 99, 235, 0.15)",
                        tension: 0.3,
                        fill: true,
                      },
                    ],
                  }}
                  options={{ plugins: { legend: { display: true } }, scales: { y: { beginAtZero: true } } }}
                />
              ) : (
                <EmptyChart />
              )}
            </ChartCard>

            <ChartCard title="Compras vs Ventas (PEN)">
              {salesSeries.data.some((v) => v > 0) || purchaseSeries.data.some((v) => v > 0) ? (
                <Bar
                  data={{
                    labels: monthLabels,
                    datasets: [
                      { label: "Ventas", data: salesSeries.data, backgroundColor: "#2563eb" },
                      { label: "Compras", data: purchaseSeries.data, backgroundColor: "#10b981" },
                    ],
                  }}
                  options={{
                    plugins: { legend: { display: true } },
                    responsive: true,
                    scales: { y: { beginAtZero: true } },
                  }}
                />
              ) : (
                <EmptyChart />
              )}
            </ChartCard>
          </div>

          <div className="grid lg:grid-cols-2 gap-6">
            <ChartCard title="Estados de facturas">
              {invoiceSeries.total > 0 ? (
                <Doughnut
                  data={{
                    labels: ["Borrador", "Abierta", "Pagada", "Cancelada"],
                    datasets: [
                      {
                        data: [invoiceSeries.draft, invoiceSeries.open, invoiceSeries.paid, invoiceSeries.cancelled],
                        backgroundColor: ["#cbd5e1", "#fb923c", "#10b981", "#ef4444"],
                      },
                    ],
                  }}
                  options={{ plugins: { legend: { position: "bottom" } } }}
                />
              ) : (
                <EmptyChart />
              )}
            </ChartCard>

            <ChartCard title="Top órdenes recientes">
              {sales.length ? (
                <div className="divide-y">
                  {sales
                    .slice(0, 5)
                    .sort((a, b) => new Date(b.fecha) - new Date(a.fecha))
                    .map((so) => (
                      <div key={so.id} className="py-2 flex items-center justify-between text-sm">
                        <div>
                          <div className="font-semibold">OV #{so.id}</div>
                          <div className="text-gray-500">{new Date(so.fecha).toLocaleDateString()}</div>
                        </div>
                        <div className="text-right">
                          <div className="font-semibold">{money(so.total)}</div>
                          <div className="text-gray-500 capitalize">{stateLabel(so.estado)}</div>
                        </div>
                      </div>
                    ))}
                </div>
              ) : (
                <EmptyChart />
              )}
            </ChartCard>
          </div>
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

function ChartCard({ title, children }) {
  return (
    <div className="bg-white rounded shadow p-4 border border-gray-100">
      <div className="text-sm font-semibold mb-2">{title}</div>
      {children}
    </div>
  );
}

function EmptyChart() {
  return <div className="text-sm text-gray-500 py-6 text-center">Aún no hay datos suficientes.</div>;
}

function money(v) {
  if (v === null || v === undefined) return "—";
  return new Intl.NumberFormat("es-PE", { style: "currency", currency: "PEN", maximumFractionDigits: 0 }).format(v);
}

function buildLabels(...collections) {
  const months = new Set();
  collections.flat().forEach((item) => {
    if (!item?.fecha) return;
    const d = new Date(item.fecha);
    const label = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, "0")}`;
    months.add(label);
  });
  const sorted = Array.from(months).sort();
  return sorted.length ? sorted : [new Date().toISOString().slice(0, 7)];
}

function groupByMonthTotal(items, labels) {
  const map = {};
  labels.forEach((l) => (map[l] = 0));
  items.forEach((item) => {
    if (!item?.fecha) return;
    const d = new Date(item.fecha);
    const key = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, "0")}`;
    map[key] = (map[key] || 0) + Number(item.total || 0);
  });
  return { data: labels.map((l) => map[l] || 0) };
}

function groupInvoices(invoices) {
  const acc = { draft: 0, open: 0, paid: 0, cancelled: 0, openTotal: 0, total: invoices.length };
  invoices.forEach((i) => {
    const state = Number(i.estado ?? 0);
    if (state === 0) acc.draft += 1;
    else if (state === 1) {
      acc.open += 1;
      acc.openTotal += Number(i.total || 0);
    } else if (state === 2) acc.paid += 1;
    else if (state === 3) acc.cancelled += 1;
  });
  return acc;
}

function calcAvg(orders) {
  if (!orders?.length) return 0;
  const sum = orders.reduce((s, o) => s + Number(o.total || 0), 0);
  return sum / orders.length;
}

function stateLabel(code) {
  const map = {
    0: "Cotización",
    1: "Confirmada",
    2: "Entregada",
    3: "Facturada",
    4: "Pagada",
    5: "Cancelada",
  };
  return map[code] || "—";
}
