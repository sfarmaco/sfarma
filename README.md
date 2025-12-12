# Sfarma (ERP + POS + Tienda online para farmacia)

Stack:
- Backend: .NET 8 Web API, Entity Framework Core, PostgreSQL, JWT
- Frontend: Vite + React + React Router + Axios + TailwindCSS

## Estructura
```
backend/        # API .NET 8 (Controllers, Services, Repositories, DTOs, Models, Data)
frontend/       # Vite + React + Tailwind, pantallas base y servicios
```

## Backend (.NET 8)
1) Preparar:
```bash
cd backend
dotnet restore
```
2) Migraciones locales (si usas EF Migrations):
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
3) Ejecutar API:
```bash
dotnet run
```
Swagger: https://localhost:5001/swagger (ajusta puerto si difiere).

### Configuración
`backend/appsettings.json`:
```json
"ConnectionStrings": {
  "Default": "Host=localhost;Port=5432;Database=sfarma_db;Username=postgres;Password=postgres"
},
"Jwt": {
  "Key": "TU_LLAVE_SECRETA_LARGA_Y_SEGURA",
  "Issuer": "Sfarma.Api",
  "Audience": "Sfarma.Client"
}
```
Actualiza credenciales de DB y la llave JWT antes de producción.

### Módulos y endpoints actuales
- Auth: `api/auth/login`, `api/auth/register`
- Contactos: `api/partners`
- Productos: `api/productos`
- Proveedores: `api/proveedores`
- Usuarios (Admin): `api/usuarios`
- Ventas: `api/saleorders` (estados: Quote, Confirmed, Delivered, Invoiced, Paid, Cancelled) y `POST /api/saleorders/{id}/invoice` para generar factura cliente.
- Compras: `api/purchaseorders` (estados: Draft, Sent, Received, Invoiced, Paid, Cancelled) y `POST /api/purchaseorders/{id}/invoice` para factura de proveedor.
- Facturas: `api/invoices` (Customer/Vendor, estados: Draft, Open, Paid, Cancelled)
- Inventario: ubicaciones y pickings `api/inventory/*` (pickings Incoming/Outgoing/Internal; estados Draft, Reserved, Done). Ajusta stock al completar.
- Dashboard: `GET /api/dashboard` (KPI de ventas, inventario, compras).

Flujo stock:
- Venta Delivered descuenta stock.
- Compra Received suma stock.
- Picking Done ajusta stock según tipo.
- Factura desde OV: `/api/saleorders/{id}/invoice`.

Roles sugeridos por endpoint:
- Admin: acceso total.
- Sales: saleorders, partners, productos.
- Purchase: purchaseorders, proveedores, productos.
- Inventory: inventory (locations/pickings).
- Accounting: invoices.

## Frontend (Vite + React)
1) Instalar:
```bash
cd frontend
npm install
```
2) Dev:
```bash
npm run dev   # http://localhost:5173
```
3) Build:
```bash
npm run build
```

### Ajustes
- API base: `frontend/src/services/api.js` usa `VITE_API_BASE_URL` (o `http://localhost:5070/api` como fallback).
- Estilos: `src/index.css`, `tailwind.config.js`.
- AuthContext guarda token/usuario en localStorage.

## Ejecutar ambos
- Backend: `cd backend && dotnet run`
- Frontend: `cd frontend && npm run dev`

## Despliegue
- Backend: `dotnet publish -c Release` y desplegar con PostgreSQL. En Railway: root `/backend`, build `dotnet restore && dotnet publish -c Release -o out`, start `./out/Sfarma.Api`, env `ASPNETCORE_URLS=http://0.0.0.0:${PORT}`.
- Frontend: `npm run build` y servir `frontend/dist`. Si lo integras en el backend, copia `frontend/dist/*` a `backend/wwwroot` y redeploy.

## CORS (backend Program.cs)
- Permitidos: `http://localhost:5173`, `https://localhost:5173`, `https://sfarma-production.up.railway.app`

## Próximos pasos sugeridos
- Usuarios de prueba (roles):
  - Admin: `admin@sfarma.com` / `Admin123!`
  - Ventas: `ventas@sfarma.com` / `Sales123!`
  - Compras: `compras@sfarma.com` / `Purchase123!`
  - Inventario: `inventario@sfarma.com` / `Inventory123!`
  - Contabilidad: `contabilidad@sfarma.com` / `Accounting123!`
- Roles/permisos finos por módulo.
- Lotes/series y alertas de vencimiento.
- Dashboards y reportes PDF.
- POS/CRM/Proyectos/Encuestas/Firma/HR.
