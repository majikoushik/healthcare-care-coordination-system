using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HealthcareCareCoordination.SharedKernel.Audit;

public class HttpAuditLogger(
    HttpClient httpClient, 
    IHttpContextAccessor httpContextAccessor, 
    ILogger<HttpAuditLogger> logger,
    string sourceService) : IAuditLogger
{
    public async Task LogEventAsync(
        string eventType, 
        string entityType, 
        string entityId, 
        string action, 
        string outcome, 
        string summary, 
        string severity = "Info", 
        object? metadata = null, 
        string? patientId = null, 
        string? providerId = null, 
        string? actorType = null, 
        string? actorId = null, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var correlationId = httpContextAccessor.HttpContext?.Items["X-Correlation-ID"]?.ToString() 
                                ?? httpContextAccessor.HttpContext?.TraceIdentifier 
                                ?? Guid.NewGuid().ToString("N");

            var payload = new
            {
                CorrelationId = correlationId,
                EventType = eventType,
                EntityType = entityType,
                EntityId = entityId,
                PatientId = patientId,
                ProviderId = providerId,
                ActorType = actorType ?? "System",
                ActorId = actorId ?? sourceService,
                Action = action,
                Outcome = outcome,
                Summary = summary,
                SourceService = sourceService,
                Severity = severity,
                Metadata = metadata
            };

            // Fire and forget or await. We will await for strict consistency in this demo.
            var response = await httpClient.PostAsJsonAsync("/api/v1/audit-events", payload, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to emit audit event {EventType}. Status: {StatusCode}", eventType, response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            // Do not break the core workflow if audit logging fails
            logger.LogError(ex, "Exception while attempting to emit audit event {EventType}", eventType);
        }
    }
}
