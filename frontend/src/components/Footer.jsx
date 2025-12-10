import { Link } from "react-router-dom";

const socials = [
  { name: "Facebook", href: "https://facebook.com/sfarma", icon: "👍" },
  { name: "Instagram", href: "https://instagram.com/sfarma", icon: "📸" },
  { name: "X", href: "https://x.com/sfarma", icon: "🐦" },
  { name: "LinkedIn", href: "https://linkedin.com/company/sfarma", icon: "💼" },
];

export default function Footer() {
  return (
    <footer className="bg-gray-900 text-gray-100 mt-12">
      <div className="max-w-6xl mx-auto px-6 py-10 grid md:grid-cols-4 gap-6">
        <div>
          <div className="flex items-center gap-2 mb-2">
            <img src="/logo.png" alt="Sfarma" className="h-10 w-10 sm:h-12 sm:w-12 object-contain" />
          </div>
          <p className="text-sm text-gray-300">
            Farmacia online + ERP para mayoristas y minoristas. Slogan: “Sfarma, tu farmacia en la nube”.
          </p>
        </div>
        <div>
          <h4 className="font-semibold mb-2">Tienda</h4>
          <ul className="space-y-1 text-sm">
            <li><Link to="/catalogo" className="hover:text-white">Productos</Link></li>
            <li><Link to="/promos" className="hover:text-white">Promociones</Link></li>
            <li><Link to="/carrito" className="hover:text-white">Carrito</Link></li>
          </ul>
        </div>
        <div>
          <h4 className="font-semibold mb-2">ERP</h4>
          <ul className="space-y-1 text-sm">
            <li>Dashboards y KPIs</li>
            <li>Calidad y trazabilidad</li>
            <li>Portal proveedores</li>
          </ul>
        </div>
        <div>
          <h4 className="font-semibold mb-2">Redes y contacto</h4>
          <div className="flex gap-3 mb-3">
            {socials.map((s) => (
              <a key={s.name} href={s.href} className="text-2xl hover:opacity-80" aria-label={s.name}>
                {s.icon}
              </a>
            ))}
          </div>
          <p className="text-sm text-gray-300">WhatsApp: +51 999-888-777</p>
          <p className="text-sm text-gray-300">Correo: contacto@sfarma.com</p>
        </div>
      </div>
    </footer>
  );
}
