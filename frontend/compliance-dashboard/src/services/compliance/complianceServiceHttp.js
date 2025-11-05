import apiHandler from "../utils/apiHandler.js";

const API_BASE_URL = import.meta.env.VITE_COMPLIANCE_API_URL;
const complianceServiceHttp = apiHandler(API_BASE_URL);

export default complianceServiceHttp;
