import { useEffect, useState } from "react";
import getComplianceService from "../../services/compliance/getComplianceService";
import SectionTitle from "../../components/SectionTitle";
import ComplianceCardDashboard from "./ComplianceCardDashboard";
import ComplianceItemsDashboard from "./ComplianceItemsDashboard";
import RecentActivitiesDashboard from "./RecentActivitiesDashboard";
import SiderbarDashboard from "./SidebarDashboard";

const MainContentDashboard = () => {
  // State to manage compliance rules and active navigation item
  const [complianceRuleList, setComplianceRuleList] = useState([]);
  const [activeNavItem, setActiveNavItem] = useState("");

  useEffect(() => {
    const fetchData = async () => {
      setComplianceRuleList(await getComplianceService());
    };
    fetchData();
  }, []);

  useEffect(() => {
    if (complianceRuleList.length > 0) {
      setActiveNavItem(complianceRuleList[0]);
    }
  }, [complianceRuleList]);

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
            <SectionTitle name={activeNavItem} />
            <ComplianceCardDashboard />
            <RecentActivitiesDashboard />
            <ComplianceItemsDashboard />
          </>
        )}
      </main>
    </div>
  );
};

export default MainContentDashboard;
