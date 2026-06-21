using HealthcareCareCoordination.Notification.Worker.Domain;

namespace HealthcareCareCoordination.Notification.Worker.Infrastructure;

public interface INotificationRepository
{
    Task<NotificationRecord> CreateAsync(NotificationRecord notification, CancellationToken cancellationToken = default);
    Task<NotificationRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificationRecord>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificationRecord>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificationRecord>> GetByRelatedEntityAsync(string entityType, string entityId, CancellationToken cancellationToken = default);
    Task UpdateAsync(NotificationRecord notification, CancellationToken cancellationToken = default);
}
