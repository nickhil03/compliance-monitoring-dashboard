import apiHandler from "../utils/apiHandler.js";

const API_BASE_URL = import.meta.env.VITE_AUTH_API_URL;
const userServiceHttp = apiHandler(API_BASE_URL);

export default userServiceHttp;
