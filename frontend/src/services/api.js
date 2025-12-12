import axios from "axios";

const inferredBase =
  import.meta.env.VITE_API_BASE_URL ||
  (typeof window !== "undefined" ? `${window.location.origin}/api` : "http://localhost:5070/api");

const api = axios.create({
  // Prioriza variable de entorno; si no existe, usa mismo dominio del frontend o fallback local
  baseURL: inferredBase,
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export default api;
