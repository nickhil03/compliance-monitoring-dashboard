import { useState, useEffect } from "react";
import getRecentActivitiesService from "../../services/compliance/getRecentActivitiesService";

const RecentActivitiesDashboard = () => {
  const [recentActivities, setRecentActivities] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      setRecentActivities(await getRecentActivitiesService());
    };
    fetchData();
  }, []);

  return (
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
              <p className="text-gray-500 text-sm">{activity.description}</p>
            </div>
            <span className="text-gray-400 text-xs">{activity.date}</span>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default RecentActivitiesDashboard;
