using HealthcareCareCoordination.ClinicalAI;
using HealthcareCareCoordination.Cosmos;
using FluentValidation;
using HealthcareCareCoordination.ClinicalInsights.Api.Features;
using HealthcareCareCoordination.ClinicalInsights.Api.Infrastructure;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.SharedKernel;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "ClinicalInsights.Api";

builder.Services.AddHealthcareApiFoundation(serviceName);

// Configure AI Provider Abstraction (Mock for local dev)
builder.Services.AddSingleton<IClinicalTextAnalyzer, MockClinicalTextAnalyzer>();

// Configure Repository (Mock Cosmos DB container)
builder.Services.AddSingleton<IClinicalInsightRepository, MockClinicalInsightRepository>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();
app.UseHealthcareApiFoundation();

app.MapGet("/api/v1/clinical-insights/readiness", (HttpContext context) =>
{
    var metadata = new ServiceMetadata(serviceName, "Synthetic clinical note insight readiness", "Azure Cosmos DB + Mock AI Provider");
    return Results.Ok(new ApiResponse<ServiceMetadata>(
        metadata,
        context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
        DateTimeOffset.UtcNow));
});

app.MapClinicalInsightEndpoints();

app.Run();
