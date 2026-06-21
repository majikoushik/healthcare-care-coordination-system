using HealthcareCareCoordination.ClinicalAI;
using HealthcareCareCoordination.Cosmos;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.SharedKernel;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "ClinicalInsights.Api";

builder.Services.AddHealthcareApiFoundation(serviceName);
builder.Services.AddSingleton<IClinicalTextAnalyzer, MockClinicalTextAnalyzer>();

var app = builder.Build();
app.UseHealthcareApiFoundation();

app.MapGet("/api/v1/clinical-insights/readiness", (HttpContext context) =>
{
    var metadata = new
    {
        Service = new ServiceMetadata(serviceName, "Synthetic clinical note insight generation", "Azure Cosmos DB"),
        Cosmos = new CosmosContainerOptions("ClinicalInsights", "/patientId", "Clinical notes and AI insight documents"),
        AiProvider = nameof(MockClinicalTextAnalyzer)
    };
    return Results.Ok(new ApiResponse<object>(
        metadata,
        context.Items[CorrelationIdMiddleware.HeaderName]?.ToString() ?? context.TraceIdentifier,
        DateTimeOffset.UtcNow));
});

app.Run();
