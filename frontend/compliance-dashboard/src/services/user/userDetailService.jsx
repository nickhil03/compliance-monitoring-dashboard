import complianceServiceHttp from "../compliance/complianceServiceHttp";

const userDetailService = async () => {
  try {
    const res = await complianceServiceHttp.get("/user/details");

    if (!res.data || Object.keys(res.data).length === 0) {
      localStorage.removeItem("token");
      throw new Error("User details not found");
    }
    return res.data;
  } catch (err) {
    console.error("Error fetching user details:", err);
  }
};

export default userDetailService;
