namespace HealthcareCareCoordination.Messaging;

public sealed record IntegrationEvent(
    Guid EventId,
    string EventType,
    DateTimeOffset OccurredAt,
    string CorrelationId,
    string EntityType,
    string EntityId,
    string? PatientId);
