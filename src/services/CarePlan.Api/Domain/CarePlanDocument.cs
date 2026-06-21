namespace HealthcareCareCoordination.CarePlan.Api.Domain;

public class CarePlanGoal
{
    public Guid GoalId { get; set; } = Guid.NewGuid();
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset? TargetDate { get; set; }
    public GoalStatus Status { get; set; } = GoalStatus.NotStarted;
    public Priority Priority { get; set; } = Priority.Medium;
}

public class CarePlanTask
{
    public Guid TaskId { get; set; } = Guid.NewGuid();
    public string TaskTitle { get; set; } = string.Empty;
    public string TaskDescription { get; set; } = string.Empty;
    public DateTimeOffset? DueDate { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Pending;
    public Priority Priority { get; set; } = Priority.Medium;
    public string AssignedTo { get; set; } = string.Empty;
    public DateTimeOffset? CompletedTimestamp { get; set; }
}

public class CarePlanDocument
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public Guid ProviderId { get; set; }
    public Guid? RelatedAppointmentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ClinicalSummary { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public DateTimeOffset? FollowUpDate { get; set; }
    public CarePlanStatus Status { get; set; } = CarePlanStatus.Draft;
    public List<CarePlanGoal> Goals { get; set; } = new();
    public List<CarePlanTask> Tasks { get; set; } = new();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
}
