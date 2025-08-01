import userServiceHttp from "./userServiceHttp";

const userLoginService = async (username, password) => {
  try {
    const res = await userServiceHttp.post("login", {
      username,
      password,
    });
    const token = res.data.token;
    return token;
  } catch (err) {
    console.log("Invalid credentials");
  }
};

export default userLoginService;
