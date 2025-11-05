import { useEffect, useState } from "react";
import getComplianceService from "../../services/compliance/getComplianceService";
import SectionTitle from "../../components/SectionTitle";
import ComplianceCardDashboard from "./ComplianceCardDashboard";
import ComplianceItemsDashboard from "./ComplianceItemsDashboard";
import RecentActivitiesDashboard from "./RecentActivitiesDashboard";
import SiderbarDashboard from "./SidebarDashboard";

const MainContentDashboard = () => {
  // State to manage compliance rules and active navigation item
  const [isLoading, setIsLoading] = useState(true);
  const [complianceRuleList, setComplianceRuleList] = useState([]);
  const [activeNavItem, setActiveNavItem] = useState("");

  useEffect(() => {
    const fetchData = async () => {
      try {
        setComplianceRuleList(await getComplianceService());
      } catch (error) {
        console.error("Fetch failed:", error);
        // Handle error state if needed
      } finally {
        setIsLoading(false); // Set to false when fetch is complete (success or fail)
      }
    };
    fetchData();
  }, []);

  useEffect(() => {
    if (complianceRuleList.length > 0) {
      setActiveNavItem(complianceRuleList[0]);
    }
  }, [complianceRuleList]);

  if (isLoading) {
    return <div>Loading compliance rules...</div>;
  }

  if (complianceRuleList.length === 0) {
    return <div>No compliance rules found.</div>;
  }

  return (
    <div className="flex flex-1">
      <SiderbarDashboard
        complianceRuleList={complianceRuleList}
        setActiveNavItem={setActiveNavItem}
        activeNavItem={activeNavItem}
      />

      <main className="flex-1 p-6 bg-gray-50 overflow-auto">
        {activeNavItem && (
          <>
            <SectionTitle activeNavItem={activeNavItem} />
            <ComplianceCardDashboard rule={activeNavItem} />
            <RecentActivitiesDashboard />
            <ComplianceItemsDashboard rule={activeNavItem} />
          </>
        )}
      </main>
    </div>
  );
};

export default MainContentDashboard;
