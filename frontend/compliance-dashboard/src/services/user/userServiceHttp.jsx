import axios from "axios";

const userServiceHttp = axios.create({
  baseURL: "https://localhost:7016/api",
  headers: {
    "Content-Type": "application/json",
  },
});

export default userServiceHttp;
