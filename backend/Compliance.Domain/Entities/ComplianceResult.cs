namespace Compliance.Domain.Entities
{
    public class ComplianceResult
    {
        public string Id { get; set; } = null!;
        public string RuleName { get; set; } = null!;
        public bool IsCompliant { get; set; }
    }
}
