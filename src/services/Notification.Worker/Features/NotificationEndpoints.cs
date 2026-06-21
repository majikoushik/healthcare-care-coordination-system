using FluentValidation;
using HealthcareCareCoordination.Notification.Worker.Domain;
using HealthcareCareCoordination.Notification.Worker.Infrastructure;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.SharedKernel.Audit;
using HealthcareCareCoordination.Observability;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareCareCoordination.Notification.Worker.Features;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/notifications").WithTags("Notifications");

        group.MapPost("/", async (
            CreateNotificationRequest request,
            IValidator<CreateNotificationRequest> validator,
            INotificationRepository repository,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage });
                return Results.ValidationProblem(errors);
            }

            var notification = new NotificationRecord
            {
                PatientId = request.PatientId,
                ProviderId = request.ProviderId,
                RelatedEntityType = request.RelatedEntityType,
                RelatedEntityId = request.RelatedEntityId,
                NotificationType = request.NotificationType,
                Channel = request.Channel,
                RecipientType = request.RecipientType,
                RecipientReference = request.RecipientReference,
                Subject = request.Subject,
                MessageSummary = request.MessageSummary
            };

            await repository.CreateAsync(notification, cancellationToken);

            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Created($"/api/v1/notifications/{notification.Id}", new ApiResponse<NotificationRecord>(notification, cid, DateTimeOffset.UtcNow));
        });

        group.MapGet("/", async (INotificationRepository repository, HttpContext context, CancellationToken cancellationToken) =>
        {
            var results = await repository.GetAllAsync(cancellationToken);
            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<IEnumerable<NotificationRecord>>(results, cid, DateTimeOffset.UtcNow));
        });

        group.MapGet("/{id:guid}", async (Guid id, INotificationRepository repository, HttpContext context, CancellationToken cancellationToken) =>
        {
            var result = await repository.GetByIdAsync(id, cancellationToken);
            if (result == null) return Results.NotFound();
            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<NotificationRecord>(result, cid, DateTimeOffset.UtcNow));
        });

        group.MapGet("/related/{relatedEntityType}/{relatedEntityId}", async (string relatedEntityType, string relatedEntityId, INotificationRepository repository, HttpContext context, CancellationToken cancellationToken) =>
        {
            var results = await repository.GetByRelatedEntityAsync(relatedEntityType, relatedEntityId, cancellationToken);
            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<IEnumerable<NotificationRecord>>(results, cid, DateTimeOffset.UtcNow));
        });

        group.MapPost("/{id:guid}/simulate-send", async (
            Guid id, 
            ISimulatedNotificationDispatcher dispatcher, 
            IAuditLogger auditLogger,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var result = await dispatcher.DispatchAsync(id, cancellationToken);
            if (!result.Success && result.Message == "Notification not found.")
            {
                return Results.NotFound();
            }

            if (!result.Success)
            {
                return Results.BadRequest(new ProblemDetails { Detail = result.Message });
            }

            var response = new SimulateSendResponse(
                result.Record.Id,
                result.Record.Status,
                result.Record.Channel,
                result.Record.AttemptCount,
                result.Record.SentAt ?? DateTimeOffset.UtcNow,
                result.Message
            );

            await auditLogger.LogEventAsync(
                eventType: "NotificationSimulatedSent",
                entityType: "Notification",
                entityId: result.Record.Id.ToString(),
                action: "SimulateSend",
                outcome: "Success",
                summary: $"Notification was successfully simulated for channel {result.Record.Channel}.",
                metadata: new { 
                    Channel = result.Record.Channel, 
                    Status = result.Record.Status, 
                    RecipientType = result.Record.RecipientType 
                },
                patientId: result.Record.PatientId?.ToString(),
                providerId: result.Record.ProviderId?.ToString(),
                cancellationToken: cancellationToken);

            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<SimulateSendResponse>(response, cid, DateTimeOffset.UtcNow));
        });

        var patientGroup = app.MapGroup("/api/v1/patients").WithTags("Notifications");
        patientGroup.MapGet("/{patientId:guid}/notifications", async (Guid patientId, INotificationRepository repository, HttpContext context, CancellationToken cancellationToken) =>
        {
            var results = await repository.GetByPatientIdAsync(patientId, cancellationToken);
            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<IEnumerable<NotificationRecord>>(results, cid, DateTimeOffset.UtcNow));
        });
    }
}
