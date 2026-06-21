using HealthcareCareCoordination.Notification.Worker.Domain;
using HealthcareCareCoordination.Notification.Worker.Infrastructure;

namespace HealthcareCareCoordination.Notification.Worker.Features;

public interface ISimulatedNotificationDispatcher
{
    Task<SimulatedNotificationResult> DispatchAsync(Guid notificationId, CancellationToken cancellationToken = default);
}

public record SimulatedNotificationResult(bool Success, string Message, NotificationRecord Record);

public class SimulatedNotificationDispatcher(
    INotificationRepository repository,
    ILogger<SimulatedNotificationDispatcher> logger) : ISimulatedNotificationDispatcher
{
    public async Task<SimulatedNotificationResult> DispatchAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        var record = await repository.GetByIdAsync(notificationId, cancellationToken);
        if (record == null)
        {
            return new SimulatedNotificationResult(false, "Notification not found.", default!);
        }

        try
        {
            // Transition state to queue if requested
            if (record.Status == NotificationStatus.Requested)
            {
                record.Queue();
                await repository.UpdateAsync(record, cancellationToken);
            }

            // Simulate the dispatch
            logger.LogInformation("Simulating dispatch for NotificationId: {NotificationId}. Channel: {Channel}", record.Id, record.Channel);
            
            // Simulating a delay
            await Task.Delay(100, cancellationToken);

            record.MarkSimulatedSent();
            await repository.UpdateAsync(record, cancellationToken);

            logger.LogInformation("Successfully simulated notification dispatch. No real external communication was sent.");
            
            return new SimulatedNotificationResult(true, "Notification delivery was simulated. No real email, SMS, or external message was sent.", record);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to simulate dispatch for NotificationId: {NotificationId}", notificationId);
            
            if (record.Status == NotificationStatus.Queued)
            {
                record.MarkSimulatedFailed(ex.Message);
                await repository.UpdateAsync(record, cancellationToken);
            }
            
            return new SimulatedNotificationResult(false, $"Simulation failed: {ex.Message}", record);
        }
    }
}
