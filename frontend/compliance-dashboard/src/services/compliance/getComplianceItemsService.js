import complianceServiceHttp from "./complianceServiceHttp";

const getComplianceItemsService = async (ruleId) => {
  if (!ruleId || !String(ruleId).trim()) {
    console.warn("getComplianceItemsService: ruleId is required");
    return [];
  }

  try {
    const response = await complianceServiceHttp.get(
      `/ComplianceItems/${encodeURIComponent(ruleId)}`
    );
    return response.data || [];
  } catch (err) {
    if (err && err.response) {
      const { status, data } = err.response;
      if (status === 404) {
        // No items found for the rule
        return [];
      }
      if (status === 400) {
        // Bad request (e.g. missing ruleId) — treat as no data client-side
        console.warn(
          "Bad request fetching compliance items:",
          data || err.message
        );
        return [];
      }
    }

    console.error("Error fetching compliance items:", err);
    throw err;
  }
};

export default getComplianceItemsService;
