import { createContext, useState, useContext } from "react";

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [isLoggedIn, setIsLoggedIn] = useState(
    !!localStorage.getItem("token") && localStorage.getItem("token") !== ""
  );

  // Function to handle successful login from the Login component
  const handleLoginSuccess = (loginStatus) => {
    setIsLoggedIn(loginStatus);
  };

  // Function to handle logout
  const handleLogout = () => {
    debugger;
    setIsLoggedIn(false);
    localStorage.removeItem("token");
  };

  const authKeyValue = {
    isLoggedIn,
    handleLoginSuccess,
    handleLogout,
  };

  return (
    <AuthContext.Provider value={authKeyValue}>{children}</AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
