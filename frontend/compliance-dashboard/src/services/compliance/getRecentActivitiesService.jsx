import complianceServiceHttp from "./complianceServiceHttp";

const getRecentActivitiesService = async () => {
  try {
    const { data } = await complianceServiceHttp.get(
      "/Activity/getRecentActivities"
    );
    if (!Array.isArray(data)) {
      throw new Error("Invalid response format");
    }
    return data;
  } catch (err) {
    console.error("Failed to fetch recent activities:", err);
  }
};

export default getRecentActivitiesService;
