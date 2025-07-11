const SectionTitle = ({ activeNavItem }) => {
  console.log("SectionTitle activeNavItem:", activeNavItem);
  const title =
    typeof activeNavItem === "string" && activeNavItem.length > 0
      ? activeNavItem.charAt(0).toUpperCase() + activeNavItem.slice(1)
      : "";

  return <h2 className="text-3xl font-semibold text-gray-800 mb-6">{title}</h2>;
};

export default SectionTitle;
