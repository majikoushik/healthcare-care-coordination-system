using HealthcareCareCoordination.Cosmos;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.SharedKernel;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "CarePlan.Api";

builder.Services.AddHealthcareApiFoundation(serviceName);

var app = builder.Build();
app.UseHealthcareApiFoundation();

app.MapGet("/api/v1/care-plans/readiness", (HttpContext context) =>
{
    var metadata = new
    {
        Service = new ServiceMetadata(serviceName, "Care plan documents and follow-up tasks", "Azure Cosmos DB"),
        Cosmos = new CosmosContainerOptions("CarePlans", "/patientId", "Care plan documents")
    };
    return Results.Ok(new ApiResponse<object>(
        metadata,
        context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
        DateTimeOffset.UtcNow));
});

app.Run();
