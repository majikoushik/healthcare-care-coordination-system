using HealthcareCareCoordination.Cosmos;
using FluentValidation;
using HealthcareCareCoordination.CarePlan.Api.Features;
using HealthcareCareCoordination.CarePlan.Api.Infrastructure;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.SharedKernel;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "CarePlan.Api";

builder.Services.AddHealthcareApiFoundation(serviceName);

// Use a singleton mock repository to simulate an Azure Cosmos DB container for local development
builder.Services.AddSingleton<ICarePlanRepository, MockCarePlanRepository>();
builder.Services.AddSingleton<IFollowUpTaskRepository, MockFollowUpTaskRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();
app.UseHealthcareApiFoundation();

app.MapGet("/api/v1/care-plans/readiness", (HttpContext context) =>
{
    var metadata = new ServiceMetadata(serviceName, "Care goals, instructions, and follow-up tasks", "Azure Cosmos DB (Mocked locally)");
    return Results.Ok(new ApiResponse<ServiceMetadata>(
        metadata,
        context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
        DateTimeOffset.UtcNow));
});

app.MapCarePlanEndpoints();

app.Run();
