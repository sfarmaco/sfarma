import axios from "axios";

const api = axios.create({
  // Ajusta al puerto real que usa tu API (ver salida de dotnet run)
  baseURL: "http://localhost:5070/api",
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export default api;
