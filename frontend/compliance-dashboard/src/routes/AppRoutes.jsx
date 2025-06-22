import { useState } from "react";
import { Routes, Route, Navigate, useNavigate } from "react-router-dom";
import LoginPage from "../pages/LoginPage";
import DashboardPage from "../pages/Dashboard/DashboardPage";
import Home from "../pages/Home";
import Signup from "../pages/Signup";

const AppRoutes = () => {
  const navigate = useNavigate();
  // State to manage authentication status
  const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem("token"));

  // Function to handle successful login from the Login component
  const handleLoginSuccess = (loginStatus) => {
    setIsLoggedIn(loginStatus);
  };

  // Function to handle logout
  const handleLogout = () => {
    setIsLoggedIn(false);
    localStorage.removeItem("token"); // Clear the stored JWT
    navigate("/login");
  };

  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route
        path="/login"
        element={<LoginPage onLoginSuccess={handleLoginSuccess} />}
      />
      <Route
        path="/dashboard"
        element={
          isLoggedIn ? (
            <DashboardPage isLoggedIn={isLoggedIn} onLogout={handleLogout} />
          ) : (
            // Redirect to login if not authenticated
            <Navigate to="/login" replace />
          )
        }
      />
      <Route
        path="*"
        element={<Navigate to={isLoggedIn ? "/dashboard" : "/login"} replace />}
      />
      <Route path="/signup" element={<Signup />} />
    </Routes>
  );
};

export default AppRoutes;
