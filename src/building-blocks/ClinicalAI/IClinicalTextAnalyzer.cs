namespace HealthcareCareCoordination.ClinicalAI;

public interface IClinicalTextAnalyzer
{
    Task<ClinicalInsightResult> AnalyzeAsync(ClinicalNoteRequest request, CancellationToken cancellationToken = default);
}

public sealed record ClinicalNoteRequest(string PatientId, string SyntheticClinicalNote, string CorrelationId);

public sealed record ClinicalInsightResult(
    string ProviderName,
    IReadOnlyCollection<string> ExtractedTerms,
    IReadOnlyCollection<string> FollowUpSuggestionsForReview,
    string HumanReviewNotice);
