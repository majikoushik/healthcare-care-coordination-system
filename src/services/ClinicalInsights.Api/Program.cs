using HealthcareCareCoordination.ClinicalAI;
using HealthcareCareCoordination.Cosmos;
using FluentValidation;
using HealthcareCareCoordination.ClinicalInsights.Api.Features;
using HealthcareCareCoordination.ClinicalInsights.Api.Infrastructure;
using HealthcareCareCoordination.Observability;
using HealthcareCareCoordination.SharedKernel;
using HealthcareCareCoordination.SharedKernel.Audit;
using HealthcareCareCoordination.Security;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "ClinicalInsights.Api";

builder.AddHealthcareObservability(serviceName);
builder.Services.AddHealthcareApiFoundation(serviceName);

// Bind Configuration
builder.Services.Configure<ClinicalAIConfiguration>(builder.Configuration.GetSection(ClinicalAIConfiguration.SectionName));
var aiConfig = builder.Configuration.GetSection(ClinicalAIConfiguration.SectionName).Get<ClinicalAIConfiguration>() ?? new ClinicalAIConfiguration();

// Configure AI Provider Abstraction
if (aiConfig.ProviderMode == "AzureAIConfigured")
{
    if (string.IsNullOrWhiteSpace(aiConfig.AzureLanguage.Endpoint) || string.IsNullOrWhiteSpace(aiConfig.AzureLanguage.Key))
    {
        throw new InvalidOperationException("ClinicalAI:AzureLanguage:Endpoint and ClinicalAI:AzureLanguage:Key must be configured when ClinicalAI:ProviderMode is AzureAIConfigured. Use Key Vault, user secrets, or environment variables; do not hardcode secrets.");
    }

    // Use Azure Provider if explicitly configured
    builder.Services.AddSingleton<IClinicalTextAnalyzer, AzureTextAnalyticsForHealthProvider>();
}
else
{
    // Fallback or explicit Mock
    builder.Services.AddSingleton<IClinicalTextAnalyzer, MockClinicalTextAnalyzer>();
}

// Configure Repository (Mock Cosmos DB container)
builder.Services.AddSingleton<IClinicalInsightRepository, MockClinicalInsightRepository>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddAuditLogging(serviceName);
builder.Services.AddHealthcareSecurity(builder.Configuration);

var app = builder.Build();
app.UseHealthcareApiFoundation(serviceName);
app.UseHealthcareSecurity();

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
