namespace HealthcareCareCoordination.Appointment.Api.Domain;

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public Guid ProviderId { get; set; }
    public DateTimeOffset AppointmentDateTime { get; set; }
    public string VisitReason { get; set; } = string.Empty;
    public AppointmentType Type { get; set; } = AppointmentType.Consultation;
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Requested;
    public string Notes { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
}
