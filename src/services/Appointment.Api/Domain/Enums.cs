namespace HealthcareCareCoordination.Appointment.Api.Domain;

public enum AppointmentType
{
    Consultation,
    FollowUp,
    LabReview,
    MedicationReview,
    CarePlanReview
}

public enum AppointmentStatus
{
    Requested,
    Scheduled,
    CheckedIn,
    Completed,
    Cancelled,
    NoShow
}
