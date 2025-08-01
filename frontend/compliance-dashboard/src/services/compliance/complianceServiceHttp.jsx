import axios from "axios";

const complianceServiceHttp = axios.create({
  baseURL: import.meta.env.VITE_COMPLIANCE_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

complianceServiceHttp.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default complianceServiceHttp;
