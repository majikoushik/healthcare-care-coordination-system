namespace HealthcareCareCoordination.SharedKernel.Audit;

public interface IAuditLogger
{
    Task LogEventAsync(
        string eventType,
        string entityType,
        string entityId,
        string action,
        string outcome,
        string summary,
        string severity = "Info",
        object? metadata = null,
        string? patientId = null,
        string? providerId = null,
        string? actorType = null,
        string? actorId = null,
        CancellationToken cancellationToken = default);
}
