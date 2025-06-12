const getRecentActivitiesService = async () => {
  try {
    //const res = await complianceServiceHttp.get("/Compliance/login");
    //const recentActivities = res.data.recentActivities;
    const recentActivities = [
      {
        id: 1,
        type: "Audit Scheduled",
        description: "Annual security audit scheduled for Q3.",
        date: "2025-07-15",
      },
      {
        id: 2,
        type: "Policy Update",
        description: "Data Retention Policy v2.1 published.",
        date: "2025-07-10",
      },
      {
        id: 3,
        type: "Alert",
        description: "Unauthorized access attempt detected on server X.",
        date: "2025-07-08",
      },
      {
        id: 4,
        type: "Training Completed",
        description: "All staff completed GDPR training.",
        date: "2025-07-05",
      },
    ];
    return recentActivities;
  } catch (err) {
    alert("Invalid credentials");
  }
};

export default getRecentActivitiesService;
