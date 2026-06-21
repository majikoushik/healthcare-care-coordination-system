using FluentValidation;
using HealthcareCareCoordination.Audit.Api.Domain;
using HealthcareCareCoordination.Audit.Api.Infrastructure;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.Observability;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareCareCoordination.Audit.Api.Features;

public static class AuditEndpoints
{
    public static void MapAuditEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/audit-events").WithTags("Audit");

        group.MapPost("/", async (
            CreateAuditEventRequest request,
            IValidator<CreateAuditEventRequest> validator,
            IAuditEventRepository repository,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage });
                return Results.ValidationProblem(errors);
            }

            var auditEvent = new AuditEventDocument
            {
                CorrelationId = request.CorrelationId,
                EventType = request.EventType,
                EntityType = request.EntityType,
                EntityId = request.EntityId,
                PatientId = request.PatientId,
                ProviderId = request.ProviderId,
                ActorType = request.ActorType,
                ActorId = request.ActorId,
                Action = request.Action,
                Outcome = request.Outcome,
                Summary = request.Summary,
                SourceService = request.SourceService,
                Severity = request.Severity,
                Metadata = request.Metadata
            };

            await repository.CreateAsync(auditEvent, cancellationToken);

            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Created($"/api/v1/audit-events/{auditEvent.Id}", new ApiResponse<AuditEventDocument>(auditEvent, cid, DateTimeOffset.UtcNow));
        });

        group.MapGet("/", async (IAuditEventRepository repository, HttpContext context, CancellationToken cancellationToken) =>
        {
            var results = await repository.GetAllAsync(cancellationToken);
            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<IEnumerable<AuditEventDocument>>(results, cid, DateTimeOffset.UtcNow));
        });

        group.MapGet("/{id}", async (string id, IAuditEventRepository repository, HttpContext context, CancellationToken cancellationToken) =>
        {
            var result = await repository.GetByIdAsync(id, cancellationToken);
            if (result == null) return Results.NotFound();
            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<AuditEventDocument>(result, cid, DateTimeOffset.UtcNow));
        });

        group.MapGet("/correlation/{correlationId}", async (string correlationId, IAuditEventRepository repository, HttpContext context, CancellationToken cancellationToken) =>
        {
            var results = await repository.GetByCorrelationIdAsync(correlationId, cancellationToken);
            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<IEnumerable<AuditEventDocument>>(results, cid, DateTimeOffset.UtcNow));
        });

        group.MapGet("/entity/{entityType}/{entityId}", async (string entityType, string entityId, IAuditEventRepository repository, HttpContext context, CancellationToken cancellationToken) =>
        {
            var results = await repository.GetByEntityAsync(entityType, entityId, cancellationToken);
            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<IEnumerable<AuditEventDocument>>(results, cid, DateTimeOffset.UtcNow));
        });

        group.MapGet("/event-type/{eventType}", async (string eventType, IAuditEventRepository repository, HttpContext context, CancellationToken cancellationToken) =>
        {
            var results = await repository.GetByEventTypeAsync(eventType, cancellationToken);
            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<IEnumerable<AuditEventDocument>>(results, cid, DateTimeOffset.UtcNow));
        });

        group.MapGet("/source-service/{sourceService}", async (string sourceService, IAuditEventRepository repository, HttpContext context, CancellationToken cancellationToken) =>
        {
            var results = await repository.GetBySourceServiceAsync(sourceService, cancellationToken);
            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<IEnumerable<AuditEventDocument>>(results, cid, DateTimeOffset.UtcNow));
        });

        var patientGroup = app.MapGroup("/api/v1/patients").WithTags("Audit");
        patientGroup.MapGet("/{patientId}/audit-events", async (string patientId, IAuditEventRepository repository, HttpContext context, CancellationToken cancellationToken) =>
        {
            var results = await repository.GetByPatientIdAsync(patientId, cancellationToken);
            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<IEnumerable<AuditEventDocument>>(results, cid, DateTimeOffset.UtcNow));
        });

        var providerGroup = app.MapGroup("/api/v1/providers").WithTags("Audit");
        providerGroup.MapGet("/{providerId}/audit-events", async (string providerId, IAuditEventRepository repository, HttpContext context, CancellationToken cancellationToken) =>
        {
            var results = await repository.GetByProviderIdAsync(providerId, cancellationToken);
            var cid = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier;
            return Results.Ok(new ApiResponse<IEnumerable<AuditEventDocument>>(results, cid, DateTimeOffset.UtcNow));
        });
    }
}
