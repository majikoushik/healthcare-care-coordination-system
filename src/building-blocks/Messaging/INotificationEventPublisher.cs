namespace HealthcareCareCoordination.Messaging;

public interface INotificationEventPublisher
{
    Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}
