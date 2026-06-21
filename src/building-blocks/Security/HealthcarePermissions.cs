namespace HealthcareCareCoordination.Security;

public static class HealthcarePermissions
{
    public const string PatientProfileRead = "PatientProfile.Read";
    public const string PatientProfileWrite = "PatientProfile.Write";
    
    public const string ProviderProfileRead = "ProviderProfile.Read";
    public const string ProviderProfileWrite = "ProviderProfile.Write";
    
    public const string AppointmentRead = "Appointment.Read";
    public const string AppointmentWrite = "Appointment.Write";
    public const string AppointmentStatusUpdate = "Appointment.StatusUpdate";
    
    public const string CarePlanRead = "CarePlan.Read";
    public const string CarePlanWrite = "CarePlan.Write";
    public const string CarePlanStatusUpdate = "CarePlan.StatusUpdate";
    
    public const string ClinicalInsightRead = "ClinicalInsight.Read";
    public const string ClinicalInsightAnalyze = "ClinicalInsight.Analyze";
    public const string ClinicalInsightReview = "ClinicalInsight.Review";
    
    public const string FollowUpTaskRead = "FollowUpTask.Read";
    public const string FollowUpTaskWrite = "FollowUpTask.Write";
    public const string FollowUpTaskStatusUpdate = "FollowUpTask.StatusUpdate";
    
    public const string NotificationRead = "Notification.Read";
    public const string NotificationWrite = "Notification.Write";
    public const string NotificationSimulateSend = "Notification.SimulateSend";
    
    public const string AuditRead = "Audit.Read";
    public const string AuditWrite = "Audit.Write";
    
    public const string SystemHealthRead = "SystemHealth.Read";

    public static readonly string[] All = typeof(HealthcarePermissions)
        .GetFields()
        .Where(f => f.IsLiteral && !f.IsInitOnly)
        .Select(f => f.GetValue(null)?.ToString() ?? string.Empty)
        .ToArray();
}
