# Operational Runbook

This guide outlines steps for support engineers and SREs to investigate issues and validate system health in the Healthcare Care Coordination System.

## 1. Tracing a Request
1. Obtain the `CorrelationId` from the frontend UI or user ticket.
2. In Azure Application Insights (or local Serilog output), filter traces using:
   ```kql
   traces | where customDimensions["CorrelationId"] == "YOUR_CORRELATION_ID"
   ```
3. Follow the sequence of calls across `Patient.Api`, `Appointment.Api`, etc.

## 2. Investigating Specific Workflows
- **Patient Registration Failure**: Check `Patient.Api` logs. Ensure SQL Server is accessible via the connection string. Look for `ValidationException` markers.
- **Appointment Scheduling Failure**: Verify `Patient.Api` and `Provider.Api` are reachable from `Appointment.Api`. Ensure SQL Server hasn't locked the records.
- **Care Plan Persistence Failure**: Care Plans use Cosmos DB. Verify Cosmos DB Partition Keys and RU limits haven't been exceeded. Check `CarePlan.Api` logs.
- **Clinical Insight Analysis Failure**: Check if the `MockClinicalTextAnalyzer` or `AzureTextAnalyticsForHealthProvider` is active. If Azure is active, verify `AzureLanguageKey` hasn't expired.
- **Notification Simulation Failure**: Check `Notification.Worker`. Ensure `ISimulatedNotificationDispatcher` didn't encounter fake 500s.
- **Audit Event Failure**: Audit relies on Cosmos DB append-only operations. If failing, Cosmos DB might be down.

## 3. Validating Service Health
Navigate to the React Portal -> **System Health** dashboard.
Alternatively, curl the endpoints directly:
- **Liveness**: `curl http://localhost:5080/health/live` (Checks if the container is running)
- **Readiness**: `curl http://localhost:5080/health/ready` (Checks downstream DBs)

### Validating Dependencies
- **SQL Server Readiness**: Covered by `/health/ready` in `Patient.Api`, `Provider.Api`, `Appointment.Api`.
- **Cosmos DB Readiness**: Covered by `/health/ready` in `CarePlan.Api`, `ClinicalInsights.Api`, `Audit.Api`.
- **AI Provider Mode**: Handled via `ClinicalInsights.Api`'s `/api/v1/clinical-insights/ai-provider/status`.

## 4. Privacy & Data Handling Rules
> [!CAUTION]
> Support teams MUST adhere to these rules when creating tickets or copying logs:
> - **NEVER** copy clinical note text into Jira, Slack, or logs.
> - **NEVER** copy Patient names or DOBs. Use the `PatientId` UUID.
> - **NEVER** expose API keys or Cosmos DB keys in chat. Use Azure Key Vault.
> - Logs are sanitized via structured logging. Do not enable trace-level logging on Entity Framework DbContext without masking.
