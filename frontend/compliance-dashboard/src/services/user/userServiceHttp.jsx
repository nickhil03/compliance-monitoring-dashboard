import axios from "axios";

const userServiceHttp = axios.create({
  baseURL: import.meta.env.VITE_AUTH_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export default userServiceHttp;
