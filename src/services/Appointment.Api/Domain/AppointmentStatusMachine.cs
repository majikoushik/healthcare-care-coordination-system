namespace HealthcareCareCoordination.Appointment.Api.Domain;

public static class AppointmentStatusMachine
{
    public static bool CanTransition(AppointmentStatus current, AppointmentStatus next)
    {
        return (current, next) switch
        {
            (AppointmentStatus.Requested, AppointmentStatus.Scheduled) => true,
            (AppointmentStatus.Requested, AppointmentStatus.Cancelled) => true,
            (AppointmentStatus.Scheduled, AppointmentStatus.CheckedIn) => true,
            (AppointmentStatus.Scheduled, AppointmentStatus.Cancelled) => true,
            (AppointmentStatus.Scheduled, AppointmentStatus.NoShow) => true,
            (AppointmentStatus.CheckedIn, AppointmentStatus.Completed) => true,
            (AppointmentStatus.CheckedIn, AppointmentStatus.Cancelled) => true,
            _ => false
        };
    }
}
