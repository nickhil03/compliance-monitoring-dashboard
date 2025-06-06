import axios from "axios";

const complianceServiceHttp = axios.create({
  baseURL: "https://localhost:7035/api",
  headers: {
    "Content-Type": "application/json",
  },
});

export default complianceServiceHttp;
