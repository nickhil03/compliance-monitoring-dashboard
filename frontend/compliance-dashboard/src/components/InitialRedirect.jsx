// Helper component for initial redirect (based on login status)
import { useAuth } from "../context/AuthContext";
import { Navigate } from "react-router-dom";

const InitialRedirect = () => {
  const { isLoggedIn } = useAuth();
  return <Navigate to={isLoggedIn ? "/dashboard" : "/login"} replace />;
};

export default InitialRedirect;
