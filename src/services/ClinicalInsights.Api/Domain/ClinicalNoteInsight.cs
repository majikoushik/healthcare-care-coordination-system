namespace HealthcareCareCoordination.ClinicalInsights.Api.Domain;

public class ExtractedEntity
{
    public string EntityId { get; set; } = Guid.NewGuid().ToString();
    public string Text { get; set; } = string.Empty;
    public EntityCategory Category { get; set; }
    public string? Subcategory { get; set; }
    public double ConfidenceScore { get; set; }
    public int? Offset { get; set; }
    public int? Length { get; set; }
    public string? NormalizedValue { get; set; }
}

public class ClinicalNoteInsight
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string PatientId { get; set; } = string.Empty;
    public string ProviderId { get; set; } = string.Empty;
    public string? RelatedCarePlanId { get; set; }
    public string? RelatedAppointmentId { get; set; }
    
    // Privacy note: Contains synthetic clinical text for demo only.
    public string ClinicalNoteText { get; set; } = string.Empty;
    
    public List<ExtractedEntity> ExtractedEntities { get; set; } = new();
    public List<string> SuggestedFollowUpTopics { get; set; } = new();
    public List<string> RiskIndicators { get; set; } = new();
    
    public string AiProviderName { get; set; } = string.Empty;
    public AiProviderMode AiProviderMode { get; set; }
    
    public HumanReviewStatus HumanReviewStatus { get; set; } = HumanReviewStatus.PendingReview;
    public string? ReviewedBy { get; set; }
    public DateTimeOffset? ReviewedTimestamp { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
}
