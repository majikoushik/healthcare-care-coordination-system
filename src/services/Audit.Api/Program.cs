using HealthcareCareCoordination.Cosmos;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.SharedKernel;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "Audit.Api";

builder.Services.AddHealthcareApiFoundation(serviceName);

var app = builder.Build();
app.UseHealthcareApiFoundation();

app.MapGet("/api/v1/audit/readiness", (HttpContext context) =>
{
    var metadata = new
    {
        Service = new ServiceMetadata(serviceName, "Append-only audit event query boundary", "Azure Cosmos DB"),
        Cosmos = new CosmosContainerOptions("AuditEvents", "/correlationId", "Audit and traceability events")
    };
    return Results.Ok(new ApiResponse<object>(
        metadata,
        context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
        DateTimeOffset.UtcNow));
});

app.Run();
