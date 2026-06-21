namespace HealthcareCareCoordination.CarePlan.Api.Domain;

public enum CarePlanStatus
{
    Draft,
    Active,
    OnHold,
    Completed,
    Cancelled
}

public enum GoalStatus
{
    NotStarted,
    InProgress,
    Achieved,
    NotAchieved,
    Cancelled
}

public enum TaskStatus
{
    Pending,
    InProgress,
    Completed,
    Overdue,
    Cancelled
}

public enum Priority
{
    Low,
    Medium,
    High,
    Critical
}
