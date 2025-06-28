import { useEffect, useState } from "react";
import getComplianceOverviewService from "../../services/compliance/getComplianceOverviewService";

const ComplianceCardDashboard = () => {
  const [complianceOverview, setComplianceOverview] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      setComplianceOverview(await getComplianceOverviewService());
    };
    fetchData();
  }, []);

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
      {complianceOverview.map((item, index) => (
        <div
          key={index}
          className="bg-white rounded-lg shadow-md p-6 flex flex-col items-start"
        >
          <div
            className={`w-12 h-12 rounded-full ${item.color} flex items-center justify-center text-white text-xl font-bold mb-4`}
          >
            {item.value.includes("%") ? item.value.split("%")[0] : item.value}
          </div>
          <h3 className="text-lg font-semibold text-gray-700 mb-2">
            {item.title}
          </h3>
          <p className="text-gray-500 text-sm">{item.status}</p>
        </div>
      ))}
    </div>
  );
};

export default ComplianceCardDashboard;
