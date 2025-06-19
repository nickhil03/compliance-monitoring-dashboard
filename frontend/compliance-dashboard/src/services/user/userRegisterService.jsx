import userServiceHttp from "./userServiceHttp";

const userRegisterService = async (user) => {
  try {
    const res = await userServiceHttp.post("/auth/register", user);
    const message = res.data.message;
    return message;
  } catch (err) {
    console.error("Registration failed:", err);
    throw new Error("Registration failed. Please try again.");
  }
};

export default userRegisterService;
