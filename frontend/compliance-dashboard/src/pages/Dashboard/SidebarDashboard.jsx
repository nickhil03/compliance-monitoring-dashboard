const SiderbarDashboard = ({
  complianceRuleList,
  activeNavItem,
  setActiveNavItem,
}) => {
  return (
    <nav className="w-64 bg-gray-800 text-white p-6 hidden md:block">
      <ul className="space-y-2">
        {complianceRuleList.map((rule) => (
          <li key={rule}>
            <button
              onClick={() => setActiveNavItem(rule)}
              className={`block w-full text-left py-2 px-4 rounded-md transition-colors duration-200
                    ${
                      activeNavItem === rule
                        ? "bg-indigo-600 text-white"
                        : "hover:bg-gray-700 text-gray-300"
                    }`}
            >
              {rule.charAt(0).toUpperCase() + rule.slice(1)}
            </button>
          </li>
        ))}
      </ul>
    </nav>
  );
};

export default SiderbarDashboard;
