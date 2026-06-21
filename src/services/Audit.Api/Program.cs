using HealthcareCareCoordination.Cosmos;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.Security;
using HealthcareCareCoordination.Audit.Api.Infrastructure;
using HealthcareCareCoordination.Audit.Api.Features;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "Audit.Api";

builder.AddHealthcareObservability(serviceName);
builder.Services.AddHealthcareApiFoundation(serviceName);

builder.Services.AddSingleton<IAuditEventRepository, MockAuditEventRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddHealthcareSecurity(builder.Configuration);

var app = builder.Build();
app.UseHealthcareApiFoundation(serviceName);
app.UseHealthcareSecurity();

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

app.MapAuditEndpoints();

app.Run();
