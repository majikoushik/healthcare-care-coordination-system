using System.Collections.Concurrent;
using HealthcareCareCoordination.Notification.Worker.Domain;

namespace HealthcareCareCoordination.Notification.Worker.Infrastructure;

/// <summary>
/// Mock repository for local development. In production, this would be backed by Azure Cosmos DB
/// using /patientId as the partition key.
/// </summary>
public sealed class MockNotificationRepository : INotificationRepository
{
    private readonly ConcurrentDictionary<Guid, NotificationRecord> _notifications = new();

    public Task<NotificationRecord> CreateAsync(NotificationRecord notification, CancellationToken cancellationToken = default)
    {
        _notifications.TryAdd(notification.Id, notification);
        return Task.FromResult(notification);
    }

    public Task<NotificationRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _notifications.TryGetValue(id, out var notification);
        return Task.FromResult(notification);
    }

    public Task<IEnumerable<NotificationRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_notifications.Values.OrderByDescending(x => x.CreatedAt).AsEnumerable());
    }

    public Task<IEnumerable<NotificationRecord>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        var results = _notifications.Values
            .Where(x => x.PatientId == patientId)
            .OrderByDescending(x => x.CreatedAt)
            .AsEnumerable();
            
        return Task.FromResult(results);
    }

    public Task<IEnumerable<NotificationRecord>> GetByRelatedEntityAsync(string entityType, string entityId, CancellationToken cancellationToken = default)
    {
        var results = _notifications.Values
            .Where(x => x.RelatedEntityType == entityType && x.RelatedEntityId == entityId)
            .OrderByDescending(x => x.CreatedAt)
            .AsEnumerable();
            
        return Task.FromResult(results);
    }

    public Task UpdateAsync(NotificationRecord notification, CancellationToken cancellationToken = default)
    {
        _notifications[notification.Id] = notification;
        return Task.CompletedTask;
    }
}
