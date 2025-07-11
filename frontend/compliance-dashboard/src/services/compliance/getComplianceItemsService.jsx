import complianceServiceHttp from "./complianceServiceHttp";

const getComplianceItemsService = async () => {
  try {
    const { data } = await complianceServiceHttp.get(
      "/ComplianceItems/getByRuleId",
      {
        params: { ruleId },
      }
    );

    return data.complianceItems || [];
  } catch (err) {
    console.error("Error fetching compliance items:", err);
  }
};

export default getComplianceItemsService;
