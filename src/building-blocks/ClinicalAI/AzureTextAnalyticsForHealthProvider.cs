namespace HealthcareCareCoordination.ClinicalAI;

public sealed class AzureTextAnalyticsForHealthProvider : IClinicalTextAnalyzer
{
    public Task<ClinicalInsightResult> AnalyzeAsync(ClinicalNoteRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Azure AI Language readiness is documented for future epics. Epic 0 uses MockClinicalTextAnalyzer locally and in CI.");
    }
}
