import HeaderDashboard from "./HeaderDashboard";
import MainContentDashboard from "./MainContentDashboard";
import { useAuth } from "../../context/AuthContext";
import { Navigate } from "react-router-dom";

const DashboardPage = () => {
  const { isLoggedIn } = useAuth();
  if (!isLoggedIn) {
    return <Navigate to="/login" replace />;
  }
  return (
    // Main container for the entire dashboard, using flexbox for layout
    <div className="min-h-screen bg-gray-100 font-sans antialiased flex flex-col">
      <HeaderDashboard />
      <MainContentDashboard />
    </div>
  );
};

export default DashboardPage;
