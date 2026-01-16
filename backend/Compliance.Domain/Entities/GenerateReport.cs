namespace Compliance.Domain.Entities
{
    public class GenerateReport
    {
        /// <summary>
        /// Unique identifier for the report.
        /// </summary>
        public Guid ReportId { get; init; } = Guid.NewGuid();

        /// <summary>
        /// Optional list of compliance IDs to include in the report.
        /// </summary>
        public List<string>? ComplianceIds { get; init; }

        public string? ReportName { get; set; }

        //public ReportFormat Format { get; set; }
        //public StorageLocation Location { get; set; }
        public string OutputPath { get; set; } = string.Empty;
    }
}
