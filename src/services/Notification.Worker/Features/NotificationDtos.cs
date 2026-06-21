using HealthcareCareCoordination.Notification.Worker.Domain;

namespace HealthcareCareCoordination.Notification.Worker.Features;

public record CreateNotificationRequest(
    Guid? PatientId,
    Guid? ProviderId,
    string RelatedEntityType,
    string RelatedEntityId,
    string NotificationType,
    NotificationChannel Channel,
    RecipientType RecipientType,
    string RecipientReference,
    string Subject,
    string MessageSummary
);

public record SimulateSendResponse(
    Guid NotificationId,
    NotificationStatus Status,
    NotificationChannel Channel,
    int AttemptCount,
    DateTimeOffset SentAt,
    string Message
);
