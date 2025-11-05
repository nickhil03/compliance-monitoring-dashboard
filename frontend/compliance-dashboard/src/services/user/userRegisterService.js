import userServiceHttp from "./userServiceHttp";

const userRegisterService = async (user) => {
  try {
    const res = await userServiceHttp.post("register", user);
    const message = res.data.message;
    return message;
  } catch (err) {
    console.log("Registration failed. Please try again.");
  }
};

export default userRegisterService;
