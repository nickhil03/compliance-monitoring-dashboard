import userServiceHttp from "./userServiceHttp";

const userLoginService = async (username, password) => {
  try {
    const res = await userServiceHttp.post("login", {
      username,
      password,
    });
    const accessToken = res.data.accessToken;
    return accessToken;
  } catch (err) {
    console.log("Invalid credentials", err);
    throw err;
  }
};

export default userLoginService;
