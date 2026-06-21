using HealthcareCareCoordination.CarePlan.Api.Domain;

namespace HealthcareCareCoordination.CarePlan.Api.DTOs;

public record CreateCarePlanRequest(
    Guid PatientId,
    Guid ProviderId,
    Guid? RelatedAppointmentId,
    string Title,
    string ClinicalSummary,
    string Instructions,
    DateTimeOffset? FollowUpDate,
    List<CarePlanGoalRequest> Goals,
    List<CarePlanTaskRequest> Tasks);

public record CarePlanGoalRequest(
    string Description,
    DateTimeOffset? TargetDate,
    Priority Priority);

public record CarePlanTaskRequest(
    string TaskTitle,
    string TaskDescription,
    DateTimeOffset? DueDate,
    Priority Priority,
    string AssignedTo);

public record UpdateCarePlanStatusRequest(
    CarePlanStatus Status,
    string Reason,
    string UpdatedBy);

public record AddCarePlanGoalRequest(
    string Description,
    DateTimeOffset? TargetDate,
    Priority Priority);

public record AddCarePlanTaskRequest(
    string TaskTitle,
    string TaskDescription,
    DateTimeOffset? DueDate,
    Priority Priority,
    string AssignedTo);

public record UpdateCarePlanTaskStatusRequest(
    Domain.TaskStatus Status,
    string Reason,
    string UpdatedBy);

public record UpdateCarePlanGoalStatusRequest(
    GoalStatus Status,
    string Reason,
    string UpdatedBy);

// Using the Domain model directly for responses for simplicity in MVP, but generally DTOs are preferred.
// In this case, since Cosmos DB stores the aggregate root as a JSON document, 
// the domain model often naturally maps to the API response perfectly without EF Core cyclical issues.
