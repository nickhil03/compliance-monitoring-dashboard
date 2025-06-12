//import complianceServiceHttp from "./complianceServiceHttp";

const getComplianceOverviewService = async () => {
  try {
    //const res = await complianceServiceHttp.get("/Compliance/login");
    //const complianceOverview = res.data.complianceOverview;
    const complianceOverview = [
      {
        title: "Overall Compliance",
        value: "92%",
        status: "Good",
        color: "bg-green-500",
      },
      {
        title: "Pending Audits",
        value: "3",
        status: "Action Required",
        color: "bg-yellow-500",
      },
      {
        title: "High-Risk Areas",
        value: "1",
        status: "Critical",
        color: "bg-red-500",
      },
      {
        title: "Policies Reviewed",
        value: "15/20",
        status: "Ongoing",
        color: "bg-blue-500",
      },
    ];
    return complianceOverview;
  } catch (err) {
    alert("Invalid credentials");
  }
};

export default getComplianceOverviewService;
