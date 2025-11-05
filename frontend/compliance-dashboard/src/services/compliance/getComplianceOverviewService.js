import complianceServiceHttp from "./complianceServiceHttp";

const getComplianceOverviewService = async (rule) => {
  try {
    const { data } = await complianceServiceHttp.get(
      "/ComplianceItems/getByRuleId",
      {
        params: { ruleId: rule },
      }
    );

    // Normalize response to an array so UI components can map safely.
    return data?.complianceOverview ?? [];
  } catch (err) {
    console.error("Please contact support, error:", err);
    return [];
  }
};

export default getComplianceOverviewService;
