import { createContext, useState, useContext, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { registerLogoutHandler } from "../services/utils/apiHandler.js";

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const tokenFromStorage = localStorage.getItem("accessToken") || "";
  const [isLoggedIn, setIsLoggedIn] = useState(!!tokenFromStorage);

  const navigate = useNavigate();

  // Function to handle successful login from the Login component
  const handleLoginSuccess = (loginStatus, token = null) => {
    setIsLoggedIn(loginStatus);
    if (loginStatus && token) {
      localStorage.setItem("accessToken", token);
      sessionStorage.setItem("accessToken", token); // keep token in sessionStorage (axios uses it)
    }
  };

  // Function to handle logout
  const handleLogout = () => {
    localStorage.removeItem("accessToken");
    sessionStorage.removeItem("accessToken");
    setIsLoggedIn(false);
    // Redirect to the login page after logout
    navigate("/login", { replace: true });
  };

  // Register the logout handler so apiHandler can call it (SPA nav)
  useEffect(() => {
    registerLogoutHandler(handleLogout);
    // cleanup (optional)
    return () => {
      registerLogoutHandler(null);
    };
  }, []);

  const authKeyValue = {
    isLoggedIn,
    handleLoginSuccess,
    handleLogout,
  };

  return (
    <AuthContext.Provider value={authKeyValue}>{children}</AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
