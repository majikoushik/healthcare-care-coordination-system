namespace HealthcareCareCoordination.ClinicalAI;

public sealed class MockClinicalTextAnalyzer : IClinicalTextAnalyzer
{
    public Task<ClinicalInsightResult> AnalyzeAsync(ClinicalNoteRequest request, CancellationToken cancellationToken = default)
    {
        var result = new ClinicalInsightResult(
            ProviderName: nameof(MockClinicalTextAnalyzer),
            ExtractedTerms: ["synthetic symptom mention", "synthetic medication mention"],
            FollowUpSuggestionsForReview: ["Review care plan follow-up timing with a qualified healthcare professional."],
            HumanReviewNotice: "This clinical insight is AI-assisted demo output and must be reviewed by a qualified healthcare professional before any real clinical use.");

        return Task.FromResult(result);
    }
}
