import HeaderDashboard from "./HeaderDashboard";
import MainContentDashboard from "./MainContentDashboard";

const DashboardPage = () => {
  return (
    // Main container for the entire dashboard, using flexbox for layout
    <div className="min-h-screen bg-gray-100 font-sans antialiased flex flex-col">
      <HeaderDashboard />
      <MainContentDashboard />
    </div>
  );
};

export default DashboardPage;
