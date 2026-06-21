using HealthcareCareCoordination.CarePlan.Api.Domain;

namespace HealthcareCareCoordination.CarePlan.Api.DTOs;

public record CreateFollowUpTaskRequest(
    Guid PatientId,
    Guid ProviderId,
    Guid CarePlanId,
    Guid? ClinicalInsightId,
    Guid? AppointmentId,
    string Title,
    string Description,
    FollowUpTaskType TaskType,
    Priority Priority,
    DateTimeOffset DueDate,
    string AssignedTo);

public record UpdateFollowUpTaskStatusRequest(
    FollowUpTaskStatus Status,
    string UpdatedBy,
    string? CompletionNotes);
