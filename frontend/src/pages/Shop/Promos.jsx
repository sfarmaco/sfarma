import PublicHeader from "../../components/PublicHeader";

const promos = [
  { title: "2x1 Analgésicos", detail: "Combina Paracetamol e Ibuprofeno", until: "Hoy" },
  { title: "Vitaminas -20%", detail: "Vitamina C y complejos B", until: "Esta semana" },
  { title: "Dermocosmética -15%", detail: "Productos dermatológicos seleccionados", until: "Fin de mes" },
];

export default function Promos() {
  return (
    <div className="min-h-screen bg-gray-50">
      <PublicHeader />
      <main className="max-w-5xl mx-auto px-6 py-10 space-y-6">
        <h1 className="text-2xl font-bold">Promociones</h1>
        <div className="grid md:grid-cols-3 gap-4">
          {promos.map((p) => (
            <div key={p.title} className="bg-white p-5 rounded-xl shadow border border-gray-100">
              <div className="text-xs text-blue-600 font-semibold mb-1">{p.until}</div>
              <div className="text-lg font-bold mb-1">{p.title}</div>
              <div className="text-sm text-gray-600">{p.detail}</div>
            </div>
          ))}
        </div>
      </main>
    </div>
  );
}
