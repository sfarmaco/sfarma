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
1) Crear y restaurar (ya generado):
```bash
cd backend
dotnet restore
```
2) Migraciones y BD PostgreSQL:
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

### Endpoints CRUD y Auth
- `api/auth/login`, `api/auth/register`
- `api/productos` (GET/GET{id}/POST/PUT/DELETE)
- `api/proveedores` (GET/GET{id}/POST/PUT/DELETE)
- `api/usuarios` (GET/GET{id}/POST/DELETE) — requiere rol Admin
- `api/ventas` (GET/GET{id}/POST)

## Frontend (Vite + React)
1) Instalar:
```bash
cd frontend
npm install
```
2) Correr en desarrollo:
```bash
npm run dev
```
Servirá en http://localhost:5173.

### Ajustes
- API base en `frontend/src/services/api.js` (`baseURL`).
- Estilos Tailwind en `src/index.css` y `tailwind.config.js`.
- AuthContext guarda token/usuario en localStorage.

### Scripts útiles
- `npm run dev` — entorno dev
- `npm run build` — build producción
- `npm run preview` — previsualizar build

## Ejecutar ambos proyectos
- Terminal 1: `cd backend && dotnet run`
- Terminal 2: `cd frontend && npm run dev`
Apunta `baseURL` del frontend al puerto real de la API.

## Despliegue
- Backend: `dotnet publish -c Release` y desplegar con PostgreSQL configurado.
- Frontend: `npm run build` y servir `frontend/dist` (Nginx/Apache/servicio estático). Actualiza CORS en la API si cambias dominios.
