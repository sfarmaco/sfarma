import { Link } from "react-router-dom";
import PublicHeader from "../../components/PublicHeader";

const promoTiles = [
  { title: "2x1 Analgésicos", desc: "Combina Paracetamol e Ibuprofeno", badge: "Hoy" },
  { title: "Vitaminas -20%", desc: "Defensas y bienestar familiar", badge: "Semana Salud" },
  { title: "Entrega express", desc: "Menos de 2h en zonas activas", badge: "Express" },
];

const erpHighlights = [
  { title: "Dashboards en vivo", desc: "Ventas, márgenes, rotación, rupturas y alertas de caducidad." },
  { title: "Calidad y trazabilidad", desc: "Lotes, vencimientos, control de temperatura y registros por proveedor." },
  { title: "Compras y mayoreo", desc: "Negocia por volumen, controla costos y automatiza reorden mínimo." },
  { title: "POS y tienda online", desc: "Caja rápida en sala y canal web unificado con inventario en tiempo real." },
  { title: "Perfiles y roles", desc: "Admin, vendedor, comprador, proveedor invitado; accesos separados." },
  { title: "KPIs críticos", desc: "GMROI, días de inventario, tickets promedio, devoluciones y mermas." },
];

const segments = [
  { title: "Detalle", desc: "Carrito ágil, promos, entregas express y favoritos." },
  { title: "Mayoreo", desc: "Listas por volumen, acuerdos con proveedores y precios escalonados." },
  { title: "Proveedores", desc: "Portal para subir catálogos, lotes, COA y negociar condiciones." },
];

export default function Home() {
  return (
    <div className="min-h-screen bg-gray-50">
      <PublicHeader />
      <main className="max-w-6xl mx-auto px-4 sm:px-6 py-8 sm:py-10 space-y-10 sm:space-y-14">
        <section className="bg-gradient-to-r from-blue-700 via-cyan-600 to-emerald-500 text-white rounded-3xl p-6 sm:p-10 shadow-xl flex flex-col lg:flex-row items-start gap-8">
          <div className="space-y-4 lg:w-2/3">
            <p className="uppercase text-sm tracking-wide">Farmacia · Tienda + ERP</p>
            <h1 className="text-2xl sm:text-3xl md:text-4xl font-bold leading-tight">
              Sfarma: tu farmacia en la nube, vende y controla en un solo lugar
            </h1>
            <p className="text-lg text-blue-50">
              Conecta clientes al detalle y al por mayor, integra proveedores y dirige tu operación con dashboards, KPIs y control de calidad.
            </p>
            <div className="flex flex-wrap gap-3">
              <Link
                to="/catalogo"
                className="px-5 py-3 bg-white text-blue-700 font-semibold rounded-lg shadow hover:shadow-md"
              >
                Ver catálogo
              </Link>
              <Link
                to="/login"
                className="px-5 py-3 border border-white/70 text-white font-semibold rounded-lg hover:bg-white/10"
              >
                Entrar como admin
              </Link>
              <a
                href="#erp"
                className="px-5 py-3 bg-white/15 text-white font-semibold rounded-lg border border-white/30 hover:bg-white/20"
              >
                Ver ERP
              </a>
            </div>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 w-full lg:w-1/3">
            {promoTiles.map((p) => (
              <div key={p.title} className="bg-white/10 rounded-xl p-4 border border-white/20">
                <div className="text-xs font-semibold text-blue-100 uppercase">{p.badge}</div>
                <div className="text-lg font-bold">{p.title}</div>
                <div className="text-blue-50 text-sm">{p.desc}</div>
              </div>
            ))}
          </div>
        </section>

        <section id="mayoreo" className="grid sm:grid-cols-2 md:grid-cols-3 gap-4 sm:gap-6">
          {segments.map((s) => (
            <div key={s.title} className="bg-white rounded-xl p-6 shadow hover:shadow-md transition border border-gray-100">
              <h3 className="text-xl font-semibold mb-2">{s.title}</h3>
              <p className="text-gray-600">{s.desc}</p>
            </div>
          ))}
        </section>

        <section id="erp" className="bg-white rounded-3xl p-6 sm:p-8 shadow border border-gray-100">
          <div className="flex items-center justify-between flex-wrap gap-3 mb-6">
            <div>
              <p className="text-sm text-gray-500 uppercase">ERP para farmacia</p>
              <h2 className="text-2xl font-bold">Control total: dashboards, indicadores y calidad</h2>
            </div>
            <Link
              to="/login"
              className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
            >
              Ir al panel
            </Link>
          </div>
          <div className="grid sm:grid-cols-2 md:grid-cols-3 gap-4">
            {erpHighlights.map((f) => (
              <div key={f.title} className="border border-gray-100 rounded-xl p-4 shadow-sm bg-gradient-to-br from-gray-50 to-white">
                <h4 className="font-semibold text-lg mb-1">{f.title}</h4>
                <p className="text-sm text-gray-600">{f.desc}</p>
              </div>
            ))}
          </div>
        </section>

        <section id="proveedores" className="bg-white rounded-3xl p-8 shadow border border-gray-100 grid md:grid-cols-2 gap-6">
          <div className="space-y-3">
            <p className="text-sm uppercase text-gray-500">Portal proveedores</p>
            <h3 className="text-2xl font-bold">Conecta proveedores y compradores en un solo flujo</h3>
            <p className="text-gray-600">
              Sube catálogos, lotes, COA y certificados; acepta pedidos al por mayor y coordina entregas con trazabilidad y alertas de vencimiento.
            </p>
            <div className="flex gap-3">
              <Link to="/catalogo" className="text-blue-700 font-semibold hover:underline">
                Ver catálogo público
              </Link>
              <Link to="/login" className="text-blue-700 font-semibold hover:underline">
                Ingresar como proveedor
              </Link>
            </div>
          </div>
          <div className="grid sm:grid-cols-2 gap-3">
            <div className="p-4 bg-blue-50 rounded-xl border border-blue-100">
              <h4 className="font-semibold">Precios escalonados</h4>
              <p className="text-sm text-gray-600">Listas por volumen y acuerdos de compra.</p>
            </div>
            <div className="p-4 bg-emerald-50 rounded-xl border border-emerald-100">
              <h4 className="font-semibold">Calidad y COA</h4>
              <p className="text-sm text-gray-600">Carga de lotes, certificados y fechas de caducidad.</p>
            </div>
            <div className="p-4 bg-amber-50 rounded-xl border border-amber-100">
              <h4 className="font-semibold">Alertas de stock</h4>
              <p className="text-sm text-gray-600">Reposición mínima y rotación por categoría.</p>
            </div>
            <div className="p-4 bg-gray-50 rounded-xl border border-gray-200">
              <h4 className="font-semibold">KPIs de servicio</h4>
              <p className="text-sm text-gray-600">OTIF, devoluciones, tiempos de entrega.</p>
            </div>
          </div>
        </section>

        <section className="bg-white rounded-3xl p-8 shadow border border-gray-100 grid md:grid-cols-3 gap-6" id="contacto">
          <div>
            <p className="text-sm uppercase text-gray-500">Puntos de venta</p>
            <h4 className="text-xl font-bold">Sucursales y cobertura</h4>
            <ul className="mt-3 text-sm text-gray-700 space-y-1">
              <li>Miraflores, Lima · Tel: (01) 555-1111</li>
              <li>San Isidro, Lima · Tel: (01) 555-2222</li>
              <li>Arequipa Centro · Tel: (054) 444-333</li>
            </ul>
          </div>
          <div>
            <p className="text-sm uppercase text-gray-500">Mayoristas y B2B</p>
            <h4 className="text-xl font-bold">Atención a volumen</h4>
            <p className="text-sm text-gray-700 mt-3">
              Condiciones especiales, precios escalonados y logística dedicada. Escríbenos para cotizaciones.
            </p>
            <p className="text-sm text-gray-700 mt-2">Correo: b2b@sfarma.com</p>
          </div>
          <div>
            <p className="text-sm uppercase text-gray-500">Contacto</p>
            <h4 className="text-xl font-bold">Soporte y ventas</h4>
            <p className="text-sm text-gray-700 mt-3">WhatsApp: +51 999-888-777</p>
            <p className="text-sm text-gray-700">Soporte: soporte@sfarma.com</p>
            <p className="text-sm text-gray-700">ERP demos: demos@sfarma.com</p>
          </div>
        </section>
      </main>
    </div>
  );
}
