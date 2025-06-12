import complianceServiceHttp from "../compliance/complianceServiceHttp";

const userDetailService = async () => {
  try {
    const res = await complianceServiceHttp.get("/user/details");

    if (!res.data || Object.keys(res.data).length === 0) {
      throw new Error("User details not found");
    }
    return res.data;
  } catch (err) {
    console.error("Invalid credentials");
  }
};

export default userDetailService;
