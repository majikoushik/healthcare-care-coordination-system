/**
 * Per-service API base URL configuration.
 *
 * Each service runs on a distinct port locally and in Docker Compose.
 * The React portal uses one Axios instance per service, not a shared gateway.
 *
 * Local defaults (direct dotnet run):
 *   Patient.Api      → http://localhost:5080
 *   Provider.Api     → http://localhost:5081
 *   Appointment.Api  → http://localhost:5082
 *   CarePlan.Api     → http://localhost:5083
 *   ClinicalInsights → http://localhost:5084
 *   Audit.Api        → http://localhost:5085
 *   Notification     → http://localhost:5086
 *
 * Docker Compose exposes the same ports on the host.
 * Override via VITE_* env vars in .env (copy from .env.example).
 */
export const apiConfig = {
  patientApiBaseUrl:
    import.meta.env.VITE_PATIENT_API_URL ?? "http://localhost:5080",
  providerApiBaseUrl:
    import.meta.env.VITE_PROVIDER_API_URL ?? "http://localhost:5081",
  appointmentApiBaseUrl:
    import.meta.env.VITE_APPOINTMENT_API_URL ?? "http://localhost:5082",
  carePlanApiBaseUrl:
    import.meta.env.VITE_CARE_PLAN_API_URL ?? "http://localhost:5083",
  clinicalInsightsApiBaseUrl:
    import.meta.env.VITE_CLINICAL_INSIGHTS_API_URL ?? "http://localhost:5084",
  auditApiBaseUrl:
    import.meta.env.VITE_AUDIT_API_URL ?? "http://localhost:5085",
  notificationApiBaseUrl:
    import.meta.env.VITE_NOTIFICATION_API_URL ?? "http://localhost:5086",

  correlationHeaderName: "X-Correlation-ID",
};
