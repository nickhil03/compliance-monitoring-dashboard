import axios from "axios";

const userServiceHttp = axios.create({
  baseURL: "https://localhost:7016/api", // Update to your .NET API base URL
  headers: {
    "Content-Type": "application/json",
  },
});

export default userServiceHttp;
