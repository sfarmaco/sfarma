import axios from "axios";

const api = axios.create({
  // Usa la URL de la API desde variable de entorno (Netlify VITE_API_BASE_URL) o fallback local
  baseURL: import.meta.env.VITE_API_BASE_URL || "http://localhost:5070/api",
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export default api;
