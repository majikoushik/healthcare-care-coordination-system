# Low-Level Design

> **Status**: Updated to reflect current implementation as of June 2026.
> Items marked **[Future]** are planned but not yet implemented.

---

## Service Internals — Current Implementation

### Cross-Cutting Building Blocks

All services share a common set of building blocks registered through extension methods:

| Building Block | What is implemented |
|---|---|
| `CorrelationIdMiddleware` | Reads incoming `X-Correlation-ID` header or generates a new GUID; stores in `HttpContext.Items`; echoes in response headers |
| `AddHealthcareApiFoundation` | Registers Swagger/OpenAPI, CORS, Problem Details, and structured error handling |
| `AddHealthcareObservability` | Registers Serilog structured logging with service name enrichment; Application Insights readiness |
| `AddAuditLogging` | Registers `HttpAuditLogger` which POSTs audit events to Audit.Api; reads `AuditApi__BaseUrl` from configuration (defaults to `http://localhost:5085`; use `http://audit-api:8080` in Docker) |
| `AddHealthcareSecurity` | Registers `DemoAuthenticationHandler`; reads `Security__Mode`, `Security__AllowDemoHeaders`, `Security__DefaultDemoRole` from configuration |
| Health endpoints | `GET /health/live`, `GET /health/ready`, `GET /health` registered via Microsoft.Extensions.Diagnostics.HealthChecks |

---

### Patient.Api

**Storage**: SQL Server / Azure SQL via Entity Framework Core
**Port (local)**: 5080

**Endpoints** (implemented):
- `POST /api/v1/patients` — Register a synthetic patient
- `GET /api/v1/patients` — List all patients
- `GET /api/v1/patients/{patientId}` — Get patient by ID
- `PUT /api/v1/patients/{patientId}` — Update patient contact details
- `PATCH /api/v1/patients/{patientId}/consent` — Update consent status
- `GET /api/v1/patients/{patientId}/appointments` — Cross-service patient appointment query

**Domain entities**: `Patient`, `ConsentStatus` (enum), `Gender` (enum)

**Validators** (FluentValidation):
- `RegisterPatientRequestValidator` — validates FullName (max 200), Email (format), DateOfBirth (not future), MobileNumber, Address, EmergencyContact fields, ConsentStatus, Gender enum membership
- `UpdatePatientRequestValidator` — validates update fields
- `UpdateConsentStatusRequestValidator` — validates enum membership

**Infrastructure**: `PatientDbContext` (EF Core), `PatientDbContext.EnsureCreated()` on startup

**Startup SQL connection**: Reads `ConnectionStrings__DefaultConnection` (ASP.NET Core convention). Docker Compose sets this via `ConnectionStrings__DefaultConnection` env var.

---

### Provider.Api

**Storage**: SQL Server / Azure SQL via Entity Framework Core
**Port (local)**: 5081

**Endpoints** (implemented):
- `POST /api/v1/providers` — Register a synthetic provider
- `GET /api/v1/providers` — List all providers
- `GET /api/v1/providers/{providerId}` — Get provider by ID
- `PUT /api/v1/providers/{providerId}` — Update provider profile
- `PATCH /api/v1/providers/{providerId}/availability` — Update availability status
- `GET /api/v1/providers/specialty/{specialty}` — Filter by specialty

**Domain enums**: `Specialty`, `AvailabilityStatus`

**Validators**: `RegisterProviderRequestValidator`, `UpdateProviderRequestValidator`, `UpdateAvailabilityStatusRequestValidator`

---

### Appointment.Api

**Storage**: SQL Server / Azure SQL via Entity Framework Core
**Port (local)**: 5082

**Endpoints** (implemented):
- `POST /api/v1/appointments` — Schedule an appointment
- `GET /api/v1/appointments` — List appointments
- `GET /api/v1/appointments/{appointmentId}` — Get appointment
- `PATCH /api/v1/appointments/{appointmentId}/status` — Transition appointment status

**Domain**: `AppointmentStatusMachine.CanTransition(current, next)` — enforces valid status transitions:
- `Requested → Scheduled | Cancelled`
- `Scheduled → CheckedIn | Cancelled | NoShow`
- `CheckedIn → Completed | Cancelled`
- Terminal states: `Completed`, `Cancelled`, `NoShow` (no further transitions allowed)

---

### CarePlan.Api

**Storage**: Azure Cosmos DB — represented by `MockCarePlanRepository` and `MockFollowUpTaskRepository` (in-memory singleton) for local development
**Port (local)**: 5083

**Endpoints** (implemented):
- `POST /api/v1/care-plans` — Create a care plan document
- `GET /api/v1/care-plans/{carePlanId}` — Get care plan
- `GET /api/v1/patients/{patientId}/care-plans` — List care plans by patient
- `PATCH /api/v1/care-plans/{carePlanId}/status` — Status transition
- `POST /api/v1/care-plans/{carePlanId}/goals` — Add a goal
- `PATCH /api/v1/care-plans/{carePlanId}/goals/{goalId}/status` — Update goal status
- `POST /api/v1/care-plans/{carePlanId}/tasks` — Add follow-up task
- `PATCH /api/v1/care-plans/{carePlanId}/tasks/{taskId}/status` — Update task status

**Status machines**:
- `CarePlanStatusMachine.CanTransition` — Draft → Active | Cancelled; Active → OnHold | Completed | Cancelled; OnHold → Active | Cancelled
- `FollowUpTaskStatusMachine.CanTransition` — Pending → InProgress | Completed | Cancelled | Overdue; InProgress → Completed | Cancelled | Overdue; Overdue → InProgress | Completed | Cancelled

**[Future]**: Replace mock in-memory repositories with real Azure Cosmos DB SDK integration.

---

### ClinicalInsights.Api

**Storage**: Azure Cosmos DB — represented by `MockClinicalInsightRepository` (in-memory) for local development
**Port (local)**: 5084

**AI Provider abstraction** (`IClinicalTextAnalyzer`):
- `MockClinicalTextAnalyzer` — Default for local development and CI. Returns deterministic synthetic output. Always includes `HumanReviewNotice`.
- `AzureTextAnalyticsForHealthProvider` — **Readiness placeholder only.** Does NOT make real Azure AI Language API calls. Simulates the expected output shape. To integrate real Azure AI Language, replace the simulation with `TextAnalyticsClient.AnalyzeHealthcareEntitiesAsync()` and configure credentials via Key Vault.

**Endpoints** (implemented):
- `POST /api/v1/clinical-insights/analyze` — Submit synthetic note for AI-assisted insight extraction
- `GET /api/v1/clinical-insights/{insightId}` — Get insight
- `GET /api/v1/patients/{patientId}/clinical-insights` — List insights by patient
- `PATCH /api/v1/clinical-insights/{insightId}/review-status` — Update human review status
- `GET /api/v1/clinical-insights/ai-provider/status` — Returns AI provider mode

**Responsible AI**: All output includes `HumanReviewNotice`. Output is explicitly labeled as demo/synthetic. No real clinical decision-making claim is made.

**[Future]**: Real Azure AI Language SDK integration with credentials from Azure Key Vault.

---

### Audit.Api

**Storage**: Azure Cosmos DB — represented by `MockAuditEventRepository` (in-memory) for local development
**Port (local)**: 5085

**Endpoints** (implemented):
- `POST /api/v1/audit/events` — Record an audit event
- `GET /api/v1/audit/events` — List all audit events
- `GET /api/v1/audit/events/{auditEventId}` — Get event
- `GET /api/v1/audit/events/correlation/{correlationId}` — Trace by correlation ID
- `GET /api/v1/audit/entity/{entityType}/{entityId}` — Events by entity
- `GET /api/v1/audit/patient/{patientId}` — Events by patient

**Design**: Audit records intentionally exclude full clinical note text and sensitive health details. All services emit audit events via `HttpAuditLogger` which POSTs to Audit.Api. `AuditApi__BaseUrl` is configurable (see `.env.example`).

---

### Notification.Worker

**Storage**: Azure Cosmos DB — represented by `MockNotificationRepository` (in-memory)
**Port (local)**: 5086

**Endpoints** (implemented):
- `POST /api/v1/notifications` — Create a notification request
- `GET /api/v1/notifications` — List notifications
- `GET /api/v1/notifications/{notificationId}` — Get notification
- `POST /api/v1/notifications/{notificationId}/simulate-send` — Simulate delivery

**Design**: Notification delivery is simulated (email, SMS, portal). No real delivery provider is connected in this demo. Notification bodies do not include sensitive clinical content.

---

## API Response Envelope

All APIs return consistent response envelopes:

```json
{
  "data": {},
  "correlationId": "string",
  "timestamp": "2026-01-01T10:00:00Z"
}
```

Errors follow RFC 7807 Problem Details:

```json
{
  "type": "https://example.com/problems/validation-error",
  "title": "Validation failed",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "correlationId": "string",
  "errors": {}
}
```

---

## Demo RBAC

The security model is a demo Role-Based Access Control layer, not production identity.

**Roles**: `Patient`, `Provider`, `CareCoordinator`, `Admin`, `Auditor`, `System`

**Mode**: `Security__Mode=Demo` — `DemoAuthenticationHandler` reads the demo role from the `X-Demo-User-Role` header and injects permission claims.

**[Future]**: Replace `DemoAuthenticationHandler` with real Azure Entra ID JWT validation. The permission policy definitions (`AddAuthorization`) are already structured to accept JWT permission claims.

---

## Frontend API Routing

The React portal uses per-service Axios clients (not a single gateway):

| Feature module | Service | Default local URL |
|---|---|---|
| Patient management | Patient.Api | http://localhost:5080 |
| Provider management | Provider.Api | http://localhost:5081 |
| Appointment scheduling | Appointment.Api | http://localhost:5082 |
| Care plans, follow-up tasks | CarePlan.Api | http://localhost:5083 |
| Clinical insights | ClinicalInsights.Api | http://localhost:5084 |
| Audit events | Audit.Api | http://localhost:5085 |
| Notifications | Notification.Worker | http://localhost:5086 |

Configure via `VITE_*_API_URL` environment variables in `.env` (copy from `.env.example`).

**[Future]**: Consider adding an Azure API Management gateway or Nginx reverse proxy for production deployments to avoid CORS complexity and enable centralized rate limiting and authentication.
