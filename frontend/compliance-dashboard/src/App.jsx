import "./App.css";
import AppRoutes from "./routes/AppRoutes"; // Importing the AppRoutes component

const App = () => {
  return (
    // Main container for the entire application
    <div className="min-h-screen bg-gray-100 font-sans antialiased flex flex-col">
      <script src="https://cdn.tailwindcss.com"></script>
      <AppRoutes />
    </div>
  );
};

export default App;
