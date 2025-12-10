import useAuth from "../hooks/useAuth";

export default function Header() {
  const { user, logout } = useAuth();
  return (
    <header className="flex justify-between items-center p-4 bg-white shadow">
      <h1 className="text-xl font-semibold">Sfarma</h1>
      <div className="flex items-center gap-3">
        <span>{user?.nombre}</span>
        <button
          onClick={logout}
          className="px-3 py-1 bg-red-500 text-white rounded"
        >
          Salir
        </button>
      </div>
    </header>
  );
}
