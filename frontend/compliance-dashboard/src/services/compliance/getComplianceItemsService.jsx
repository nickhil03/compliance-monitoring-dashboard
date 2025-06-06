//import complianceServiceHttp from "./complianceServiceHttp";

const getComplianceItemsService = () => {
  try {
    //const res = await complianceServiceHttp.get("/Compliance/");
    //const complianceItems = res.data.complianceItems;
    const complianceItems = [
      {
        id: "GDPR-001",
        name: "Data Privacy Impact Assessment",
        status: "Compliant",
        owner: "Legal Team",
        dueDate: "N/A",
      },
      {
        id: "ISO-27001-005",
        name: "Information Security Policy Review",
        status: "Pending Review",
        owner: "IT Security",
        dueDate: "2025-08-01",
      },
      {
        id: "HIPAA-010",
        name: "Patient Data Access Log Audit",
        status: "Non-Compliant",
        owner: "Compliance Officer",
        dueDate: "2025-07-20",
      },
      {
        id: "PCI-DSS-003",
        name: "Payment Gateway Vulnerability Scan",
        status: "Compliant",
        owner: "DevOps",
        dueDate: "N/A",
      },
      {
        id: "SOX-002",
        name: "Financial Reporting Controls Check",
        status: "In Progress",
        owner: "Finance Dept.",
        dueDate: "2025-09-01",
      },
    ];
    return complianceItems;
  } catch (err) {
    alert("Invalid credentials");
  }
};

export default getComplianceItemsService;
