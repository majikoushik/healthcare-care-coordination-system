using HealthcareCareCoordination.ClinicalAI;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace ClinicalInsights.Api.Tests;

/// <summary>
/// Validates that the Azure Text Analytics for Health readiness placeholder
/// returns simulated output and always includes the human review notice.
///
/// Important: AzureTextAnalyticsForHealthProvider is a READINESS PLACEHOLDER.
/// It does NOT make real Azure AI Language API calls. Real integration requires
/// a valid Azure AI Language endpoint and key configured via Key Vault or
/// user secrets — and must never be committed to source control.
/// </summary>
public class AzureProviderReadinessTests
{
    [Fact]
    public async Task AzureProvider_ReturnsDeterministicSimulatedOutput()
    {
        var provider = CreateProvider();
        var request = new ClinicalNoteRequest("patient-demo-1", "Synthetic note only.", "corr-readiness-test");

        var result = await provider.AnalyzeAsync(request);

        // The placeholder must return a non-empty list of extracted terms
        Assert.NotNull(result.ExtractedTerms);
        Assert.NotEmpty(result.ExtractedTerms);
    }

    [Fact]
    public async Task AzureProvider_AlwaysIncludesHumanReviewNotice()
    {
        var provider = CreateProvider();
        var request = new ClinicalNoteRequest("patient-demo-2", "Synthetic note only.", "corr-readiness-test-2");

        var result = await provider.AnalyzeAsync(request);

        // The human review notice is mandatory — this is the Responsible AI safety boundary
        Assert.False(string.IsNullOrWhiteSpace(result.HumanReviewNotice),
            "HumanReviewNotice must never be empty — responsible AI safeguard");
        Assert.Contains("not real medical output", result.HumanReviewNotice, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AzureProvider_ProviderNameIdentifiesAsAzureProvider()
    {
        var provider = CreateProvider();
        var request = new ClinicalNoteRequest("patient-demo-3", "Synthetic note only.", "corr-readiness-test-3");

        var result = await provider.AnalyzeAsync(request);

        Assert.Equal("AzureTextAnalyticsForHealth", result.ProviderName);
    }

    [Fact]
    public async Task AzureProvider_IncludesFollowUpSuggestions()
    {
        var provider = CreateProvider();
        var request = new ClinicalNoteRequest("patient-demo-4", "Synthetic note only.", "corr-readiness-test-4");

        var result = await provider.AnalyzeAsync(request);

        Assert.NotNull(result.FollowUpSuggestionsForReview);
        Assert.NotEmpty(result.FollowUpSuggestionsForReview);
    }

    private static AzureTextAnalyticsForHealthProvider CreateProvider()
    {
        var config = new ClinicalAIConfiguration();
        var options = Options.Create(config);
        var logger = NullLogger<AzureTextAnalyticsForHealthProvider>.Instance;
        return new AzureTextAnalyticsForHealthProvider(options, logger);
    }
}
