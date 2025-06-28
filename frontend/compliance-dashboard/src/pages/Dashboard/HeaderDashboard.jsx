import { useEffect, useState } from "react";
import userDetailService from "../../services/user/userDetailService";
import { useAuth } from "../../context/AuthContext";

const HeaderDashboard = () => {
  const { onLogout } = useAuth();

  const [userDetails, setUserDetails] = useState({ username: "", name: "" });

  useEffect(() => {
    const fetchData = async () => {
      setUserDetails(await userDetailService());
    };
    fetchData();
  }, []);

  return (
    <header className="bg-white shadow-sm p-4 flex items-center justify-between">
      <h1 className="text-2xl font-bold text-gray-800">Compliance Dashboard</h1>
      <div className="flex items-center space-x-4">
        {/* User Avatar Placeholder */}
        <div className="w-10 h-10 rounded-full bg-gray-300 flex items-center justify-center text-gray-600 font-semibold">
          {userDetails.username}
        </div>
        <span className="text-gray-700 hidden sm:block">
          {userDetails.name}
        </span>
        {/* Logout Button */}
        <button
          onClick={onLogout}
          className="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-2 transition duration-150 ease-in-out"
        >
          Logout
        </button>
      </div>
    </header>
  );
};

export default HeaderDashboard;
