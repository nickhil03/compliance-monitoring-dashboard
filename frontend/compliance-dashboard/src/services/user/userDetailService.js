import complianceServiceHttp from "../compliance/complianceServiceHttp";

const userDetailService = async () => {
  try {
    const res = await complianceServiceHttp.get("userdetails");

    if (!res.data || Object.keys(res.data).length === 0) {
      localStorage.removeItem("accessToken");
      console.log("User details not found");
    }
    return res.data;
  } catch (err) {
    console.log("Error fetching user details:", err);
  }
};

export default userDetailService;
