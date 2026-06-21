namespace HealthcareCareCoordination.Audit.Api.Domain;

public static class AuditEventOutcome
{
    public const string Success = "Success";
    public const string Failure = "Failure";
    public const string ValidationFailed = "ValidationFailed";
    public const string NotFound = "NotFound";
    public const string Unauthorized = "Unauthorized";
    public const string Forbidden = "Forbidden";
    public const string Simulated = "Simulated";
}

public static class AuditEventSeverity
{
    public const string Info = "Info";
    public const string Warning = "Warning";
    public const string Error = "Error";
    public const string Critical = "Critical";
}

public class AuditEventDocument
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CorrelationId { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string? PatientId { get; set; }
    public string? ProviderId { get; set; }
    public string ActorType { get; set; } = string.Empty;
    public string ActorId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Outcome { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string SourceService { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public object? Metadata { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
