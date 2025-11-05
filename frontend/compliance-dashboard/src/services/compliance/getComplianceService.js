import complianceServiceHttp from "./complianceServiceHttp";

const getComplianceService = async () => {
  try {
    const res = await complianceServiceHttp.get("/Compliance/get-all");
    if (!res.data || Object.keys(res.data).length === 0) {
      console.error("Compliance data not found");
    }
    var complianceList = res.data
      .filter((x) => x.isCompliant)
      .map((x) => x.ruleName);
    return complianceList;
  } catch (err) {
    console.error("No compliance data found, error:", err);
  }
};

export default getComplianceService;
