namespace Compliance.Domain.DTOs
{
    public class ComplianceResultDto
    {
        public string RuleName { get; set; } = null!;
        public bool IsCompliant { get; set; }
    }
}
