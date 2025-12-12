import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import Partners from "./pages/Partners";
import Productos from "./pages/Productos";
import Proveedores from "./pages/Proveedores";
import Ventas from "./pages/Ventas";
import Compras from "./pages/Compras";
import Inventario from "./pages/Inventario";
import Facturas from "./pages/Facturas";
import Pos from "./pages/Pos";
import ProtectedRoute from "./components/ProtectedRoute";
import Home from "./pages/Shop/Home";
import Catalog from "./pages/Shop/Catalog";
import Cart from "./pages/Shop/Cart";
import Promos from "./pages/Shop/Promos";

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Public shop */}
        <Route path="/" element={<Home />} />
        <Route path="/catalogo" element={<Catalog />} />
        <Route path="/promos" element={<Promos />} />
        <Route path="/carrito" element={<Cart />} />

        {/* Auth */}
        <Route path="/login" element={<Login />} />

        {/* Admin / ERP */}
        <Route
          path="/admin"
          element={
            <ProtectedRoute>
              <Dashboard />
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin/partners"
          element={
            <ProtectedRoute>
              <Partners />
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin/ventas"
          element={
            <ProtectedRoute>
              <Ventas />
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin/productos"
          element={
            <ProtectedRoute>
              <Productos />
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin/compras"
          element={
            <ProtectedRoute>
              <Compras />
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin/inventario"
          element={
            <ProtectedRoute>
              <Inventario />
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin/proveedores"
          element={
            <ProtectedRoute>
              <Proveedores />
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin/facturas"
          element={
            <ProtectedRoute>
              <Facturas />
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin/pos"
          element={
            <ProtectedRoute>
              <Pos />
            </ProtectedRoute>
          }
        />

        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </BrowserRouter>
  );
}
