import { AuthProvider } from "./context/AuthContext";
import { CartProvider } from "./context/CartContext";
import AppRoutes from "./routes";
import "./index.css";
import ChatWidget from "./components/ChatWidget";

export default function App() {
  return (
    <AuthProvider>
      <CartProvider>
        <div className="min-h-screen flex flex-col">
          <AppRoutes />
          <ChatWidget />
        </div>
      </CartProvider>
    </AuthProvider>
  );
}
