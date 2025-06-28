import "./App.css";
import { AuthProvider } from "./context/AuthContext";
import AppRoutes from "./routes/AppRoutes"; // Importing the AppRoutes component
import { BrowserRouter } from "react-router-dom"; // Importing BrowserRouter for routing

const App = () => {
  return (
    // Main container for the entire application
    <div className="min-h-screen bg-gray-100 font-sans antialiased flex flex-col">
      <script src="https://cdn.tailwindcss.com"></script>
      <AuthProvider>
        <BrowserRouter>
          <AppRoutes />
        </BrowserRouter>
      </AuthProvider>
    </div>
  );
};

export default App;
