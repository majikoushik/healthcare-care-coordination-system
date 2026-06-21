using HealthcareCareCoordination.ClinicalInsights.Api.Domain;

namespace HealthcareCareCoordination.ClinicalInsights.Api.DTOs;

public record AnalyzeNoteRequest(
    string PatientId,
    string ProviderId,
    string? RelatedCarePlanId,
    string? RelatedAppointmentId,
    string ClinicalNoteText);

public record UpdateReviewStatusRequest(
    HumanReviewStatus ReviewStatus,
    string ReviewedBy,
    string? ReviewNotes);
