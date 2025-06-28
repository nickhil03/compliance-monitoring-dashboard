import { Routes, Route } from "react-router-dom";
import LoginPage from "../pages/LoginPage";
import DashboardPage from "../pages/Dashboard/DashboardPage";
import ProtectedRoute from "../components/ProtectedRoute";
import FallbackComponent from "../components/FallbackComponent";
import Home from "../pages/Home";
import Signup from "../pages/Signup";
import { ErrorBoundary } from "react-error-boundary";
import InitialRedirect from "../components/InitialRedirect";

const AppRoutes = () => {
  // State to manage authentication status

  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/login" element={<LoginPage />} />
      <Route
        path="/dashboard"
        element={
          <ProtectedRoute>
            <ErrorBoundary fallback={<FallbackComponent />}>
              <DashboardPage />
            </ErrorBoundary>
          </ProtectedRoute>
        }
      />
      <Route path="*" element={<InitialRedirect />} />
      <Route path="/signup" element={<Signup />} />
    </Routes>
  );
};

export default AppRoutes;
