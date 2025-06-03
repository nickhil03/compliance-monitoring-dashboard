namespace Compliance.Domain.Messaging;
public class ComplianceEvaluationMessage
{
    public required string DataId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}