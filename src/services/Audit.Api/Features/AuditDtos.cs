namespace HealthcareCareCoordination.Audit.Api.Features;

public record CreateAuditEventRequest(
    string CorrelationId,
    string EventType,
    string EntityType,
    string EntityId,
    string? PatientId,
    string? ProviderId,
    string ActorType,
    string ActorId,
    string Action,
    string Outcome,
    string Summary,
    string SourceService,
    string Severity,
    object? Metadata
);
