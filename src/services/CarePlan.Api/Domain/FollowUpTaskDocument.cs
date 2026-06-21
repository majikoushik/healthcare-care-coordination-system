namespace HealthcareCareCoordination.CarePlan.Api.Domain;

public enum FollowUpTaskType
{
    General = 0,
    LabTest = 1,
    MedicationReview = 2,
    FollowUpAppointment = 3,
    LifestyleCounseling = 4,
    CarePlanReview = 5,
    PatientEducation = 6
}

public enum FollowUpTaskStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Overdue = 3,
    Cancelled = 4
}

public class FollowUpTaskDocument
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Partition Key
    public Guid PatientId { get; set; }
    
    // Workflow Links
    public Guid CarePlanId { get; set; }
    public Guid? ProviderId { get; set; }
    public Guid? ClinicalInsightId { get; set; }
    public Guid? AppointmentId { get; set; }
    
    // Task Payload
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public FollowUpTaskType TaskType { get; set; } = FollowUpTaskType.General;
    public Priority Priority { get; set; } = Priority.Medium; // Existing enum
    public FollowUpTaskStatus Status { get; set; } = FollowUpTaskStatus.Pending;
    
    public DateTimeOffset DueDate { get; set; }
    public string AssignedTo { get; set; } = string.Empty;
    
    // Completion details
    public DateTimeOffset? CompletedTimestamp { get; set; }
    public string? CompletionNotes { get; set; }
    
    // Audit
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
}
