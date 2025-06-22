import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import getComplianceOverviewService from "../../services/compliance/getComplianceOverviewService";
import getrecentActivitiesService from "../../services/compliance/getRecentActivitiesService";
import getComplianceItemsService from "../../services/compliance/getComplianceItemsService";
import getComplianceService from "../../services/compliance/getComplianceService";
import HeaderDashboard from "./HeaderDashboard";

const DashboardPage = ({ isLoggedIn, onLogout }) => {
  const navigate = useNavigate();

  // Effect to redirect if not logged in
  useEffect(() => {
    if (!isLoggedIn) {
      localStorage.removeItem("token");
      navigate("/login");
    }
  }, [isLoggedIn, navigate]);

  const [complianceItems, setComplianceItems] = useState([]);
  const [complianceRuleList, setComplianceRuleList] = useState([]);
  const [complianceOverview, setComplianceOverview] = useState([]);
  const [recentActivities, setRecentActivities] = useState([]);
  const [activeNavItem, setActiveNavItem] = useState("");

  useEffect(() => {
    const fetchData = async () => {
      setComplianceRuleList(await getComplianceService());
      setComplianceOverview(await getComplianceOverviewService());
      setRecentActivities(await getrecentActivitiesService());
      setComplianceItems(await getComplianceItemsService());
    };
    fetchData();
  }, []);

  useEffect(() => {
    if (complianceRuleList.length > 0) {
      setActiveNavItem(complianceRuleList[0]);
    }
  }, [complianceRuleList]);

  return (
    // Main container for the entire dashboard, using flexbox for layout
    <div className="min-h-screen bg-gray-100 font-sans antialiased flex flex-col">
      <HeaderDashboard isLoggedIn={isLoggedIn} onLogout={onLogout} />

      {/* Main Content Area - uses flex to arrange sidebar and main content */}
      <div className="flex flex-1">
        {/* Sidebar Navigation */}
        <nav className="w-64 bg-gray-800 text-white p-6 hidden md:block">
          <ul className="space-y-2">
            {complianceRuleList.map((item) => (
              <li key={item}>
                <button
                  onClick={() => setActiveNavItem(item)}
                  className={`block w-full text-left py-2 px-4 rounded-md transition-colors duration-200
                    ${
                      activeNavItem === item
                        ? "bg-indigo-600 text-white"
                        : "hover:bg-gray-700 text-gray-300"
                    }`}
                >
                  {item.charAt(0).toUpperCase() + item.slice(1)}
                </button>
              </li>
            ))}
          </ul>
        </nav>

        {/* Dashboard Main Content */}
        <main className="flex-1 p-6 bg-gray-50 overflow-auto">
          {/* Section Title */}
          <h2 className="text-3xl font-semibold text-gray-800 mb-6">
            {activeNavItem.charAt(0).toUpperCase() + activeNavItem.slice(1)}
          </h2>

          {/* Compliance Overview Cards */}
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
            {complianceOverview.map((item, index) => (
              <div
                key={index}
                className="bg-white rounded-lg shadow-md p-6 flex flex-col items-start"
              >
                <div
                  className={`w-12 h-12 rounded-full ${item.color} flex items-center justify-center text-white text-xl font-bold mb-4`}
                >
                  {item.value.includes("%")
                    ? item.value.split("%")[0]
                    : item.value}
                </div>
                <h3 className="text-lg font-semibold text-gray-700 mb-2">
                  {item.title}
                </h3>
                <p className="text-gray-500 text-sm">{item.status}</p>
              </div>
            ))}
          </div>

          {/* Recent Activities Section */}
          <div className="bg-white rounded-lg shadow-md p-6 mb-8">
            <h3 className="text-xl font-semibold text-gray-800 mb-4">
              Recent Activities
            </h3>
            <ul className="divide-y divide-gray-200">
              {recentActivities.map((activity) => (
                <li
                  key={activity.id}
                  className="py-3 flex justify-between items-center"
                >
                  <div>
                    <p className="text-gray-700 font-medium">{activity.type}</p>
                    <p className="text-gray-500 text-sm">
                      {activity.description}
                    </p>
                  </div>
                  <span className="text-gray-400 text-xs">{activity.date}</span>
                </li>
              ))}
            </ul>
          </div>

          {/* Compliance Items Table */}
          <div className="bg-white rounded-lg shadow-md p-6">
            <h3 className="text-xl font-semibold text-gray-800 mb-4">
              Compliance Items
            </h3>
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gray-50">
                  <tr>
                    <th
                      scope="col"
                      className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider rounded-tl-lg"
                    >
                      ID
                    </th>
                    <th
                      scope="col"
                      className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
                    >
                      Name
                    </th>
                    <th
                      scope="col"
                      className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
                    >
                      Status
                    </th>
                    <th
                      scope="col"
                      className="px-6 py-3 text-left text--500 uppercase tracking-wider"
                    >
                      Owner
                    </th>
                    <th
                      scope="col"
                      className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
                    >
                      Due Date
                    </th>
                    <th
                      scope="col"
                      className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider rounded-tr-lg"
                    >
                      Actions
                    </th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  {complianceItems.map((item) => (
                    <tr key={item.id}>
                      <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                        {item.id}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">
                        {item.name}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm">
                        <span
                          className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full
                          ${
                            item.status === "Compliant"
                              ? "bg-green-100 text-green-800"
                              : item.status === "Pending Review"
                              ? "bg-yellow-100 text-yellow-800"
                              : item.status === "Non-Compliant"
                              ? "bg-red-100 text-red-800"
                              : "bg-blue-100 text-blue-800"
                          }`}
                        >
                          {item.status}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">
                        {item.owner}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">
                        {item.dueDate}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                        {(item.status === "Pending Review" ||
                          item.status === "Non-Compliant" ||
                          item.status === "In Progress") && (
                          <button
                            // onClick={() =>
                            //   getComplianceSuggestions(item.name, item.status)
                            // }
                            className="text-indigo-600 hover:text-indigo-900 px-3 py-1 border border-indigo-600 rounded-md text-xs font-semibold
                                       transition duration-150 ease-in-out flex items-center justify-center
                                       disabled:opacity-50 disabled:cursor-not-allowed"
                            // disabled={isLoadingSuggestions}
                          >
                            {/* {isLoadingSuggestions ? (
                              <svg
                                className="animate-spin h-4 w-4 text-indigo-600 mr-2"
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                              >
                                <circle
                                  className="opacity-25"
                                  cx="12"
                                  cy="12"
                                  r="10"
                                  stroke="currentColor"
                                  strokeWidth="4"
                                ></circle>
                                <path
                                  className="opacity-75"
                                  fill="currentColor"
                                  d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                                ></path>
                              </svg>
                            ) : (
                              "Suggest Actions ✨"
                            )} */}
                          </button>
                        )}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
};

export default DashboardPage;
