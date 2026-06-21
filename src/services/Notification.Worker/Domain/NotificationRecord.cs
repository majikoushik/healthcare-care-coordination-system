using HealthcareCareCoordination.SharedKernel;

namespace HealthcareCareCoordination.Notification.Worker.Domain;

public enum NotificationChannel
{
    EmailSimulation,
    SmsSimulation,
    PortalSimulation
}

public enum NotificationStatus
{
    Requested,
    Queued,
    SimulatedSent,
    SimulatedFailed,
    Cancelled
}

public enum RecipientType
{
    Patient,
    Provider,
    CareCoordinator,
    Admin,
    Auditor,
    System
}

public sealed class NotificationRecord
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid? PatientId { get; init; }
    public Guid? ProviderId { get; init; }
    public string RelatedEntityType { get; init; } = default!;
    public string RelatedEntityId { get; init; } = default!;
    public string NotificationType { get; init; } = default!;
    public NotificationChannel Channel { get; init; }
    public RecipientType RecipientType { get; init; }
    public string RecipientReference { get; init; } = default!;
    public string Subject { get; init; } = default!;
    public string MessageSummary { get; init; } = default!;
    public NotificationStatus Status { get; private set; } = NotificationStatus.Requested;
    public int AttemptCount { get; private set; } = 0;
    public DateTimeOffset? LastAttemptedAt { get; private set; }
    public DateTimeOffset? SentAt { get; private set; }
    public string? FailureReason { get; private set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;

    public void Queue()
    {
        if (Status != NotificationStatus.Requested && Status != NotificationStatus.SimulatedFailed)
            throw new InvalidOperationException($"Cannot queue notification from status {Status}");
            
        Status = NotificationStatus.Queued;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkSimulatedSent()
    {
        if (Status != NotificationStatus.Queued)
            throw new InvalidOperationException($"Cannot send notification from status {Status}");

        Status = NotificationStatus.SimulatedSent;
        AttemptCount++;
        SentAt = DateTimeOffset.UtcNow;
        LastAttemptedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkSimulatedFailed(string reason)
    {
        if (Status != NotificationStatus.Queued)
            throw new InvalidOperationException($"Cannot fail notification from status {Status}");

        Status = NotificationStatus.SimulatedFailed;
        AttemptCount++;
        FailureReason = reason;
        LastAttemptedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Cancel()
    {
        if (Status == NotificationStatus.SimulatedSent)
            throw new InvalidOperationException("Cannot cancel an already sent notification");

        Status = NotificationStatus.Cancelled;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
