import Header from "../components/Header";
import Sidebar from "../components/Sidebar";

export default function Dashboard() {
  return (
    <div className="min-h-screen flex">
      <Sidebar />
      <div className="flex-1">
        <Header />
        <div className="p-6">Panel principal con KPIs (pendiente de datos).</div>
      </div>
    </div>
  );
}
