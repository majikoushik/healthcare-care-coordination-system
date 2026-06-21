namespace HealthcareCareCoordination.Security;

public static class HealthcareRoles
{
    public const string Patient = "Patient";
    public const string Provider = "Provider";
    public const string CareCoordinator = "CareCoordinator";
    public const string Admin = "Admin";
    public const string Auditor = "Auditor";
    public const string System = "System";

    public static string[] GetPermissionsForRole(string role)
    {
        return role switch
        {
            Patient => new[]
            {
                HealthcarePermissions.PatientProfileRead,
                HealthcarePermissions.AppointmentRead,
                HealthcarePermissions.CarePlanRead,
                HealthcarePermissions.FollowUpTaskRead,
                HealthcarePermissions.NotificationRead
            },
            Provider => new[]
            {
                HealthcarePermissions.PatientProfileRead,
                HealthcarePermissions.AppointmentRead,
                HealthcarePermissions.AppointmentStatusUpdate,
                HealthcarePermissions.CarePlanRead,
                HealthcarePermissions.CarePlanWrite,
                HealthcarePermissions.ClinicalInsightRead,
                HealthcarePermissions.ClinicalInsightAnalyze,
                HealthcarePermissions.FollowUpTaskRead,
                HealthcarePermissions.FollowUpTaskWrite
            },
            CareCoordinator => new[]
            {
                HealthcarePermissions.PatientProfileRead,
                HealthcarePermissions.PatientProfileWrite,
                HealthcarePermissions.ProviderProfileRead,
                HealthcarePermissions.AppointmentRead,
                HealthcarePermissions.AppointmentWrite,
                HealthcarePermissions.AppointmentStatusUpdate,
                HealthcarePermissions.CarePlanRead,
                HealthcarePermissions.CarePlanWrite,
                HealthcarePermissions.CarePlanStatusUpdate,
                HealthcarePermissions.ClinicalInsightRead,
                HealthcarePermissions.ClinicalInsightReview,
                HealthcarePermissions.FollowUpTaskRead,
                HealthcarePermissions.FollowUpTaskWrite,
                HealthcarePermissions.FollowUpTaskStatusUpdate,
                HealthcarePermissions.NotificationRead,
                HealthcarePermissions.NotificationWrite,
                HealthcarePermissions.NotificationSimulateSend
            },
            Admin => HealthcarePermissions.All, // Admin gets everything in demo
            Auditor => new[]
            {
                HealthcarePermissions.AuditRead,
                HealthcarePermissions.PatientProfileRead,
                HealthcarePermissions.ProviderProfileRead,
                HealthcarePermissions.AppointmentRead,
                HealthcarePermissions.CarePlanRead,
                HealthcarePermissions.ClinicalInsightRead,
                HealthcarePermissions.NotificationRead
            },
            System => new[]
            {
                HealthcarePermissions.AuditWrite,
                HealthcarePermissions.NotificationWrite,
                HealthcarePermissions.SystemHealthRead
            },
            _ => Array.Empty<string>()
        };
    }
}
