import complianceServiceHttp from "./complianceServiceHttp";

const getComplianceOverviewService = async () => {
  try {
    const { data } = await complianceServiceHttp.get(
      "/ComplianceItem/getByRuleId"
    );
    return data.complianceOverview || null;
  } catch (err) {
    console.error("Please contact support, error:", err);
  }
};

export default getComplianceOverviewService;
