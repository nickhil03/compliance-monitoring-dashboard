import complianceServiceHttp from "../compliance/complianceServiceHttp";

const userDetailService = async () => {
  try {
    debugger;
    const res = await complianceServiceHttp.get("/user/details");

    if (!res.data || Object.keys(res.data).length === 0) {
      localStorage.removeItem("token");
      console.log("User details not found");
    }
    return res.data;
  } catch (err) {
    console.log("Error fetching user details:", err);
  }
};

export default userDetailService;
