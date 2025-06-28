import axios from "axios";

const complianceServiceHttp = axios.create({
  baseURL: "https://localhost:7035/api",
  headers: {
    "Content-Type": "application/json",
  },
});

complianceServiceHttp.interceptors.request.use((config) => {
  debugger;
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default complianceServiceHttp;
