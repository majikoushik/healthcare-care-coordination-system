namespace HealthcareCareCoordination.ClinicalInsights.Api.Domain;

public enum AiProviderMode
{
    Mock = 0,
    AzureAIReady = 1
}

public enum HumanReviewStatus
{
    PendingReview = 0,
    Reviewed = 1,
    Approved = 2,
    Rejected = 3,
    RequiresClarification = 4
}

public enum EntityCategory
{
    Unknown = 0,
    Symptom = 1,
    Condition = 2,
    Medication = 3,
    Test = 4,
    Procedure = 5,
    FollowUp = 6,
    Lifestyle = 7,
    RiskFactor = 8
}
