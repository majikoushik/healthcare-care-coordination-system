# AGENTS.md

# Healthcare Care Coordination System — Agent Operating Guide

This file defines how AI coding agents must work in this repository.

Repository name:

```text
healthcare-care-coordination-system
```

This project is a portfolio-grade enterprise healthcare application intended to demonstrate Solution Architecture capability across:

- Healthcare domain architecture
- .NET backend engineering
- React frontend engineering
- SQL Server and Azure SQL transactional data design
- Azure Cosmos DB document and event data design
- Azure AI Language readiness for clinical note insights
- Secure healthcare data handling patterns
- Privacy and compliance-readiness patterns
- Event-driven workflows
- Audit logging and traceability
- Observability and production-readiness
- Azure cloud-native deployment architecture
- DevOps, Docker, CI/CD, and documentation maturity

This is not a hospital ERP, not a real clinical system, and not a certified healthcare product. It is an enterprise architecture portfolio project demonstrating healthcare care coordination workflows and cloud-ready engineering practices.

---

# 1. Project Vision

Build a cloud-native Healthcare Care Coordination System that demonstrates how patients, healthcare providers, and care coordinators can collaborate around appointments, care plans, follow-up tasks, clinical notes, notifications, and audit trails.

The application should support a simplified but realistic healthcare coordination journey:

1. Patient registration
2. Provider management
3. Appointment scheduling
4. Care plan management
5. Clinical note insights using mock AI locally and Azure AI Language readiness
6. Follow-up task tracking
7. Notification simulation
8. Audit logging
9. Security, privacy, and RBAC readiness
10. Observability and production-readiness
11. Azure-ready deployment architecture

The project should be useful as a GitHub portfolio project for a Solution Architect profile.

---

# 2. Target Audience

This repository should be understandable and impressive to:

- Solution Architect recruiters
- Engineering managers
- Healthcare technology reviewers
- Azure architects
- Senior .NET interviewers
- React frontend reviewers
- Cloud-native architecture teams
- Enterprise modernization teams
- Security and compliance reviewers

The code, documentation, diagrams, and README should clearly demonstrate architecture thinking, not only coding ability.

---

# 3. Core Positioning

Use this positioning throughout README and documentation:

```text
Healthcare Care Coordination System is a cloud-native healthcare platform demo built with .NET, React, TypeScript, SQL Server, Azure Cosmos DB, and Azure AI Language-ready architecture. It demonstrates patient management, provider coordination, appointment scheduling, care plan workflows, clinical note insights, follow-up task tracking, notification simulation, audit logging, observability, and secure healthcare data handling patterns.
```

Important wording:

- Use “healthcare platform demo”.
- Use “compliance-readiness patterns”.
- Use “Azure AI Language-ready”.
- Use “mock AI provider for local development”.
- Use “synthetic demo data only”.
- Use “not intended for real clinical decision-making”.

Do not claim:

- HIPAA compliant
- Clinically certified
- Production-ready medical product
- Real medical diagnosis system
- Real clinical decision support system

---

# 4. Architecture Principles

Every change must follow these principles.

## 4.1 Architecture First

Before implementing a major feature, update or create required architecture notes.

Expected artifacts may include:

- High-Level Design
- Low-Level Design
- API contracts
- Data model
- Event model
- Privacy and compliance notes
- Security architecture
- Sequence diagrams
- NFR checklist
- ADRs
- Deployment notes

## 4.2 Healthcare Privacy by Design

Treat patient and clinical data as sensitive even though this project uses synthetic data.

The implementation must avoid:

- Real patient data
- Real clinical notes
- Real medical records
- Full sensitive health data in logs
- Sensitive health details in audit metadata
- Hardcoded secrets
- Exposing backend-only secrets to the frontend

## 4.3 Enterprise-Grade Simplicity

Keep the MVP simple enough to run locally, but design it in a way that reflects enterprise thinking.

Good architecture means:

- Clear service boundaries
- Maintainability
- Testability
- Secure data handling
- Traceability
- Observability
- Azure deployment readiness
- Honest documentation of limitations

## 4.4 Domain-Oriented Design

Use healthcare domain language consistently.

Preferred terms:

- Patient
- Provider
- Appointment
- Care Plan
- Clinical Note
- Clinical Insight
- Follow-up Task
- Notification
- Audit Event
- Consent Status
- Care Coordinator

Avoid vague names such as:

- Data
- Info
- Helper
- Manager
- Processor
- CommonService

## 4.5 Polyglot Persistence

Use the right database for the right workload.

Recommended data ownership:

| Data Area | Storage | Reason |
|---|---|---|
| Patient master profile | SQL Server / Azure SQL | Structured, relational, transactional |
| Provider profile | SQL Server / Azure SQL | Structured master data |
| Appointment scheduling | SQL Server / Azure SQL | Relational consistency and scheduling integrity |
| Care plan documents | Azure Cosmos DB | Flexible document structure |
| Clinical notes | Azure Cosmos DB | Semi-structured clinical text and extracted entities |
| Follow-up tasks | Azure Cosmos DB | Embedded or document-based workflow items |
| Notification events | Azure Cosmos DB or Service Bus-ready event model | Event-style document history |
| Audit events | Azure Cosmos DB | Append-only trace/event storage |
| AI-extracted health entities | Azure Cosmos DB | Flexible JSON output |

For local development, Cosmos DB can be represented by:

- Cosmos DB Emulator where available
- Local mock repository if emulator setup is too heavy
- Container-ready placeholder with clear documentation

## 4.6 AI Provider Abstraction

Clinical note insights must use a provider abstraction.

Required local default:

```text
MockClinicalTextAnalyzer
```

Azure-ready provider:

```text
AzureTextAnalyticsForHealthProvider
```

The project must not require Azure AI credentials for local development or CI.

## 4.7 API-First Design

All backend APIs must be designed as enterprise APIs.

Each API should include:

- Clear route naming
- Request and response DTOs
- Validation rules
- Proper HTTP status codes
- Problem Details error responses
- Swagger/OpenAPI documentation
- Versioning readiness
- Consistent response patterns
- Correlation ID propagation

## 4.8 Azure-Ready by Design

Local development may use Docker Compose, SQL Server container, and mock AI providers. Production direction must align with Azure.

Preferred Azure target architecture:

- Azure Static Web Apps for React frontend
- Azure Container Apps for backend APIs and workers
- Azure Container Registry for images
- Azure SQL Database for structured data
- Azure Cosmos DB for care plans, notes, audit, notifications, and AI outputs
- Azure AI Language for Text Analytics for Health readiness
- Azure Service Bus for asynchronous messaging readiness
- Azure Key Vault for secrets
- Azure Application Insights for telemetry
- Azure Log Analytics for centralized logs
- Azure API Management as optional future gateway
- Azure Front Door as optional future edge layer

---

# 5. Technology Stack

## 5.1 Backend

Use:

- .NET 8 or current LTS .NET
- ASP.NET Core Web API
- C#
- Entity Framework Core for SQL Server modules
- Azure Cosmos DB SDK or repository abstraction for document modules
- FluentValidation or equivalent validation approach
- Swagger/OpenAPI
- Serilog or structured logging equivalent
- Health checks
- Problem Details
- Docker support
- Clean Architecture or vertical-slice architecture where appropriate

Expected backend services/modules:

- Patient.Api
- Provider.Api
- Appointment.Api
- CarePlan.Api
- ClinicalInsights.Api
- Notification.Worker or Notification.Api
- Audit.Api

## 5.2 Frontend

Use React, not Angular.

Recommended stack:

- React
- TypeScript
- Vite
- React Router
- TanStack Query
- React Hook Form
- Zod for validation
- Axios or fetch wrapper
- Tailwind CSS or Bootstrap

Preferred styling:

- Tailwind CSS for a modern dashboard look

Frontend expectations:

- Feature-based folder structure
- Typed API models
- Centralized API client
- HTTP interceptors or request middleware for correlation ID
- React Hook Form for forms
- Zod validation schemas
- TanStack Query for server state
- Professional healthcare dashboard UI
- Loading states
- Empty states
- Error states
- Responsive layout

## 5.3 Databases

Use SQL Server for structured transactional modules.

Use Azure Cosmos DB for document/event modules.

SQL Server modules:

- Patient
- Provider
- Appointment

Cosmos DB modules:

- Care Plan
- Clinical Notes
- Clinical Insights
- Follow-up Tasks
- Notifications
- Audit Events

## 5.4 Azure AI

Use Azure AI Language Text Analytics for Health readiness in the Clinical Insights module.

Local default must be mock provider.

Azure provider must be optional and configuration-driven.

Do not require Azure credentials for local development.

Do not use Azure OpenAI for the first healthcare AI feature unless explicitly requested later. Azure AI Language for Health is more domain-specific for this project.

---

# 6. Recommended Repository Structure

Use this structure unless there is a strong reason to change it.

```text
healthcare-care-coordination-system/
│
├── README.md
├── AGENTS.md
├── docker-compose.yml
├── .gitignore
├── .editorconfig
│
├── src/
│   ├── services/
│   │   ├── Patient.Api/
│   │   ├── Provider.Api/
│   │   ├── Appointment.Api/
│   │   ├── CarePlan.Api/
│   │   ├── ClinicalInsights.Api/
│   │   ├── Notification.Worker/
│   │   └── Audit.Api/
│   │
│   ├── building-blocks/
│   │   ├── SharedKernel/
│   │   ├── Observability/
│   │   ├── Security/
│   │   ├── Messaging/
│   │   ├── Compliance/
│   │   └── Persistence/
│   │
│   └── web/
│       └── healthcare-care-portal/
│
├── tests/
│   ├── Patient.Api.Tests/
│   ├── Provider.Api.Tests/
│   ├── Appointment.Api.Tests/
│   ├── CarePlan.Api.Tests/
│   ├── ClinicalInsights.Api.Tests/
│   ├── Notification.Worker.Tests/
│   └── Audit.Api.Tests/
│
├── architecture/
│   ├── README.md
│   ├── hld.md
│   ├── lld.md
│   ├── nfrs.md
│   ├── api-governance.md
│   ├── security-architecture.md
│   ├── privacy-and-compliance.md
│   ├── responsible-ai-architecture.md
│   ├── observability-architecture.md
│   ├── deployment-architecture.md
│   ├── data-model.md
│   ├── event-model.md
│   ├── polyglot-persistence-strategy.md
│   ├── clinical-insights-ai-strategy.md
│   ├── diagrams/
│   │   ├── system-context.md
│   │   ├── container-diagram.md
│   │   ├── appointment-scheduling-sequence.md
│   │   ├── care-plan-flow.md
│   │   ├── clinical-insights-flow.md
│   │   ├── audit-event-flow.md
│   │   └── azure-deployment.md
│   └── adr/
│       ├── 0001-architecture-style.md
│       ├── 0002-react-frontend.md
│       ├── 0003-polyglot-persistence.md
│       ├── 0004-sql-server-for-transactional-data.md
│       ├── 0005-cosmos-db-for-documents-and-events.md
│       ├── 0006-clinical-insights-ai-provider-abstraction.md
│       ├── 0007-mock-ai-provider-first.md
│       ├── 0008-azure-ai-language-provider-readiness.md
│       └── 0009-observability-strategy.md
│
├── docs/
│   ├── setup.md
│   ├── local-development.md
│   ├── api-contracts.md
│   ├── testing-strategy.md
│   ├── privacy-guidelines.md
│   ├── responsible-ai-guidelines.md
│   ├── operational-runbook.md
│   ├── deployment.md
│   ├── azure-deployment-guide.md
│   ├── devops-guide.md
│   ├── roadmap.md
│   └── screenshots/
│
├── infra/
│   ├── bicep/
│   └── scripts/
│
└── .github/
    └── workflows/
        ├── ci.yml
        └── azure-deploy-template.yml
```

The agent may simplify the structure during the earliest MVP foundation, but it must preserve the long-term architectural direction.

---

# 7. React Frontend Structure

Create the React app under:

```text
src/web/healthcare-care-portal/
```

Preferred structure:

```text
healthcare-care-portal/
│
├── src/
│   ├── app/
│   │   ├── App.tsx
│   │   ├── router.tsx
│   │   └── providers.tsx
│   │
│   ├── core/
│   │   ├── api/
│   │   │   ├── axiosClient.ts
│   │   │   └── apiConfig.ts
│   │   ├── hooks/
│   │   ├── types/
│   │   └── utils/
│   │
│   ├── shared/
│   │   ├── components/
│   │   ├── layout/
│   │   ├── forms/
│   │   └── ui/
│   │
│   ├── features/
│   │   ├── dashboard/
│   │   ├── patients/
│   │   ├── providers/
│   │   ├── appointments/
│   │   ├── care-plans/
│   │   ├── clinical-insights/
│   │   ├── follow-up-tasks/
│   │   ├── notifications/
│   │   ├── audit/
│   │   └── system-health/
│   │
│   ├── assets/
│   └── main.tsx
│
├── package.json
├── vite.config.ts
├── tsconfig.json
└── tailwind.config.js
```

React routes:

```text
/dashboard
/patients
/patients/new
/patients/:id
/providers
/providers/new
/providers/:id
/appointments
/appointments/new
/appointments/:id
/care-plans
/care-plans/new
/care-plans/:id
/clinical-insights
/clinical-insights/new
/clinical-insights/:id
/follow-up-tasks
/notifications
/audit
/system-health
```

---

# 8. MVP Scope

The MVP must include the following capabilities.

## 8.1 Patient Registration

A patient should be registered with basic profile information.

Sample fields:

- Patient ID
- Full name
- Date of birth
- Gender
- Email
- Mobile number
- Address
- Emergency contact name
- Emergency contact number
- Consent status
- Created timestamp
- Updated timestamp

Storage:

- SQL Server

## 8.2 Provider Management

A provider should be registered and available for appointments.

Sample fields:

- Provider ID
- Full name
- Specialty
- Email
- Mobile number
- Department
- Availability status
- Created timestamp
- Updated timestamp

Storage:

- SQL Server

## 8.3 Appointment Scheduling

Users should schedule appointments between patients and providers.

Supported statuses:

- Requested
- Scheduled
- CheckedIn
- Completed
- Cancelled
- NoShow

Storage:

- SQL Server

## 8.4 Care Plan Management

Care plans should represent ongoing coordination for a patient.

Sample fields:

- Care Plan ID
- Patient ID
- Provider ID
- Title
- Clinical summary
- Goals
- Instructions
- Follow-up date
- Status
- Tasks
- Created timestamp
- Updated timestamp

Supported statuses:

- Draft
- Active
- OnHold
- Completed
- Cancelled

Storage:

- Azure Cosmos DB

## 8.5 Clinical Note Insights

Users should submit a synthetic clinical note and receive extracted health-related insights.

Local default:

- MockClinicalTextAnalyzer

Azure-ready provider:

- AzureTextAnalyticsForHealthProvider

Sample output:

- Extracted conditions
- Extracted symptoms
- Medication-related terms
- Follow-up recommendations
- Risk indicators
- Confidence score where available
- Human review notice

Storage:

- Azure Cosmos DB

Important:

- Do not claim diagnosis.
- Do not claim clinical decision support.
- Clearly state the output is informational demo output requiring clinical review.

## 8.6 Follow-up Task Tracking

Care plans should contain follow-up tasks.

Task fields:

- Task ID
- Care Plan ID
- Task title
- Task description
- Due date
- Status
- Priority
- Assigned to

Supported task statuses:

- Pending
- InProgress
- Completed
- Overdue
- Cancelled

Storage:

- Azure Cosmos DB, preferably embedded in care plan documents or as separate documents if justified.

## 8.7 Notification Simulation

Simulate notifications for important healthcare workflow events.

Triggers:

- Patient registered
- Appointment scheduled
- Appointment cancelled
- Care plan created
- Care plan updated
- Follow-up task due
- Clinical insight generated

Channels:

- Email simulation
- SMS simulation
- Portal notification simulation

Storage:

- Azure Cosmos DB or Service Bus-ready event model

## 8.8 Audit Logging

Audit important business actions.

Events:

- Patient registered
- Patient viewed
- Provider registered
- Appointment scheduled
- Appointment status changed
- Care plan created
- Care plan updated
- Care plan task completed
- Clinical insight generated
- Notification requested
- Notification delivery simulated

Storage:

- Azure Cosmos DB

Do not store sensitive health details in audit metadata.

## 8.9 Security, Privacy, and RBAC Readiness

Document and prepare for roles:

- Patient
- Provider
- CareCoordinator
- Admin
- Auditor

Authentication may be simulated or deferred in MVP, but the architecture should document future use of:

- Azure Entra ID
- JWT authorization
- Role-based access control

## 8.10 Observability

Include:

- Correlation ID
- Structured logging
- Health checks
- Global error handling
- Problem Details
- Application Insights readiness

---

# 9. Service Boundary Guidance

## Patient.Api

Responsible for:

- Patient profile
- Contact details
- Emergency contact
- Consent status readiness

Not responsible for:

- Appointment scheduling
- Care plan document ownership
- Clinical AI processing

## Provider.Api

Responsible for:

- Provider profile
- Specialty
- Department
- Availability status readiness

Not responsible for:

- Appointment lifecycle ownership
- Care plan ownership

## Appointment.Api

Responsible for:

- Appointment creation
- Appointment status
- Patient-provider association
- Appointment timeline

Not responsible for:

- Patient master data ownership
- Provider master data ownership
- Care plan management

## CarePlan.Api

Responsible for:

- Care plan document lifecycle
- Care goals
- Follow-up instructions
- Follow-up tasks
- Care plan status

Storage:

- Cosmos DB

Not responsible for:

- Patient master data ownership
- Appointment scheduling
- AI provider integration, unless limited to linking clinical insight IDs

## ClinicalInsights.Api

Responsible for:

- Accepting synthetic clinical notes
- Calling mock AI provider locally
- Optional Azure AI Language provider readiness
- Storing extracted clinical insight results
- Returning structured insight summaries

Storage:

- Cosmos DB

Not responsible for:

- Real diagnosis
- Real medical advice
- Clinical decision-making

## Notification.Worker or Notification.Api

Responsible for:

- Notification request creation
- Notification delivery simulation
- Notification status tracking

Not responsible for:

- Real email/SMS provider delivery in MVP

## Audit.Api

Responsible for:

- Audit event recording
- Audit event querying
- Traceability by entity, patient, correlation ID, and event type

Not responsible for:

- Storing sensitive health details

---

# 10. API Design Standards

Use RESTful conventions.

Suggested endpoints:

```text
POST   /api/v1/patients
GET    /api/v1/patients
GET    /api/v1/patients/{patientId}
PUT    /api/v1/patients/{patientId}

POST   /api/v1/providers
GET    /api/v1/providers
GET    /api/v1/providers/{providerId}
PUT    /api/v1/providers/{providerId}

POST   /api/v1/appointments
GET    /api/v1/appointments
GET    /api/v1/appointments/{appointmentId}
GET    /api/v1/patients/{patientId}/appointments
PATCH  /api/v1/appointments/{appointmentId}/status

POST   /api/v1/care-plans
GET    /api/v1/care-plans/{carePlanId}
GET    /api/v1/patients/{patientId}/care-plans
PATCH  /api/v1/care-plans/{carePlanId}/status
POST   /api/v1/care-plans/{carePlanId}/tasks
PATCH  /api/v1/care-plans/{carePlanId}/tasks/{taskId}/status

POST   /api/v1/clinical-insights/analyze
GET    /api/v1/clinical-insights/{insightId}
GET    /api/v1/patients/{patientId}/clinical-insights

GET    /api/v1/notifications
GET    /api/v1/notifications/{notificationId}
GET    /api/v1/notifications/entity/{entityType}/{entityId}

GET    /api/v1/audit/events
GET    /api/v1/audit/events/{auditEventId}
GET    /api/v1/audit/entity/{entityType}/{entityId}
GET    /api/v1/audit/patient/{patientId}
GET    /api/v1/audit/correlation/{correlationId}
```

Use consistent response models.

Example success response:

```json
{
  "data": {},
  "correlationId": "string",
  "timestamp": "2026-01-01T10:00:00Z"
}
```

Example error response should follow Problem Details style:

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

# 11. React Coding Standards

## 11.1 Component Standards

React components should be:

- Small
- Typed
- Feature-scoped
- Easy to test
- Free of direct API calls
- Clear in responsibility

## 11.2 API Integration

Use a central API client.

Expected behavior:

- API base URL from environment variables
- Correlation ID header on requests
- Safe error mapping
- Typed request/response models
- TanStack Query for server state

Do not call APIs directly from page components if a feature service/hook should be used.

## 11.3 Forms

Use:

- React Hook Form
- Zod validation

Forms should include:

- Required validation
- Domain-specific validation
- User-friendly messages
- Disabled submit button while invalid or submitting
- Loading state
- Success state
- API error state

## 11.4 UI Expectations

The UI should look like a professional healthcare operations portal.

Expected design elements:

- Sidebar navigation
- Top header
- Dashboard cards
- Status badges
- Patient profile cards
- Appointment timeline
- Care plan task board
- Clinical insight summary card
- Recent audit events
- Recent notifications
- System health summary

---

# 12. Clinical AI Standards

## 12.1 Local Mock Provider

The mock provider must:

- Work without Azure credentials
- Return deterministic demo output
- Be safe for local development and CI
- Clearly identify output as mock/demo insight
- Include human review notice

## 12.2 Azure AI Language Provider Readiness

The Azure provider must:

- Be optional
- Be environment-configured
- Not require credentials for local development
- Use safe error handling
- Avoid logging full clinical notes
- Avoid exposing secrets

Suggested environment variables:

```text
AI_PROVIDER=Mock
AZURE_AI_LANGUAGE_ENDPOINT=<your-language-endpoint>
AZURE_AI_LANGUAGE_KEY=<from-key-vault-or-user-secrets>
AZURE_AI_LANGUAGE_API_VERSION=<api-version>
AZURE_AI_LANGUAGE_TIMEOUT_SECONDS=60
```

For local and CI:

```text
AI_PROVIDER=Mock
```

## 12.3 Responsible AI Notice

Clinical insights must include this concept:

```text
This clinical insight is AI-assisted demo output and must be reviewed by a qualified healthcare professional before any real clinical use.
```

## 12.4 Do Not Claim Diagnosis

The application must not claim:

- The patient has a diagnosis
- A treatment is recommended as medical advice
- The AI output is clinically approved
- The output replaces professional judgment

Use wording such as:

- Extracted entities
- Possible clinical terms
- Follow-up suggestions for review
- Demo output
- Requires healthcare professional review

---

# 13. Database Design Standards

## 13.1 SQL Server

Use SQL Server for structured modules.

Tables may include:

- Patients
- Providers
- Appointments

Each table should include:

- Primary key
- Created timestamp
- Updated timestamp
- Status where relevant
- Concurrency strategy where useful

## 13.2 Cosmos DB

Use Cosmos DB for flexible document/event modules.

Containers may include:

- CarePlans
- ClinicalInsights
- Notifications
- AuditEvents

Partition key guidance:

| Container | Suggested Partition Key |
|---|---|
| CarePlans | `/patientId` |
| ClinicalInsights | `/patientId` |
| Notifications | `/entityId` or `/patientId` |
| AuditEvents | `/correlationId` or `/patientId` depending query design |

Document partitioning decisions in architecture docs.

## 13.3 No Real Data

Use synthetic seed data only.

Avoid realistic combinations that could identify a real person.

---

# 14. Event Design Standards

Events should represent business facts that already happened.

Good event names:

- PatientRegistered
- ProviderRegistered
- AppointmentScheduled
- AppointmentStatusChanged
- CarePlanCreated
- CarePlanUpdated
- CarePlanTaskCompleted
- ClinicalInsightGenerated
- NotificationRequested
- NotificationDeliveryCompleted
- AuditEventRecorded

Avoid command-like event names:

- RegisterPatientEvent
- SendNotificationNow
- DoClinicalAnalysis

Event payloads should include:

- Event ID
- Event type
- Occurred timestamp
- Correlation ID
- Entity ID
- Patient ID where applicable
- Minimal safe business data required by consumers

Do not include full clinical note text in event payloads.

---

# 15. Observability Standards

Every backend service should include:

- Correlation ID middleware
- Structured logging
- Request logging
- Error logging
- Health check endpoint
- Application Insights readiness
- Safe clinical data logging policy

Minimum log fields:

- Timestamp
- Level
- Service name
- Correlation ID
- Event name
- Entity type
- Entity ID where relevant
- Patient ID only if safe and synthetic
- Message
- Exception details where applicable

Do not log:

- Secrets
- Full clinical notes
- Full sensitive health details
- Connection strings
- Tokens
- API keys
- Full patient address
- Full notification body if sensitive

---

# 16. Security, Privacy, and Compliance-Readiness Standards

Security must be considered in every epic.

Minimum expectations:

- Input validation
- Output safety
- Secure configuration
- No secrets in repository
- No real patient data
- No sensitive data in logs
- No sensitive health details in audit metadata
- CORS configured intentionally
- Future authentication and authorization hooks
- Secure headers where practical
- OWASP awareness
- Privacy-by-design documentation

Future authentication direction:

- Azure Entra ID or Entra External ID
- JWT-based API authorization
- Role-based access control

Possible roles:

- Patient
- Provider
- CareCoordinator
- Admin
- Auditor

For MVP, authentication may be simulated or deferred, but architecture notes must explain the future direction.

Compliance wording:

Use:

```text
compliance-readiness patterns
```

Do not use:

```text
HIPAA compliant
certified compliant
clinically certified
```

---

# 17. Azure Deployment Direction

All deployment work should align with this target architecture.

## 17.1 Frontend

Preferred:

- Azure Static Web Apps

Alternative:

- Azure Storage Static Website with CDN readiness

## 17.2 Backend APIs and Workers

Preferred:

- Azure Container Apps
- Azure Container Registry

## 17.3 Relational Database

Preferred:

- Azure SQL Database

## 17.4 NoSQL Database

Preferred:

- Azure Cosmos DB for NoSQL

## 17.5 AI Provider

Preferred:

- Azure AI Language
- Text Analytics for Health readiness

## 17.6 Messaging

Preferred future:

- Azure Service Bus

## 17.7 Secrets

Preferred:

- Azure Key Vault
- Managed Identity

## 17.8 Monitoring

Preferred:

- Azure Application Insights
- Azure Log Analytics
- Azure Monitor

## 17.9 Infrastructure as Code

Use Bicep as preferred Azure IaC unless a future epic explicitly selects Terraform.

Infrastructure files should be placed under:

```text
infra/bicep/
```

---

# 18. Documentation Requirements

Documentation is part of the product.

Every major epic must update documentation.

Minimum documentation files:

## README.md

Should include:

- Project overview
- Architecture summary
- Tech stack
- Features
- Polyglot persistence summary
- Azure AI Language readiness
- How to run locally
- How to run tests
- API documentation link
- Privacy and responsible AI note
- Azure deployment direction
- Screenshots when UI exists

## architecture/hld.md

Should include:

- Business context
- System context
- Container view
- Major components
- Service responsibilities
- Integration points
- Key quality attributes

## architecture/lld.md

Should include:

- Service internals
- Main classes/modules
- API flow
- Database interaction
- Validation flow
- Error handling
- Event handling

## architecture/polyglot-persistence-strategy.md

Should include:

- SQL Server responsibilities
- Cosmos DB responsibilities
- Partition key choices
- Transaction boundaries
- Query patterns
- Data consistency notes

## architecture/clinical-insights-ai-strategy.md

Should include:

- Mock provider
- Azure AI Language provider readiness
- Responsible AI notice
- Safe logging policy
- Limitations
- Human review requirement

## architecture/privacy-and-compliance.md

Should include:

- Synthetic data policy
- Sensitive data handling
- Audit logging policy
- Safe logging policy
- RBAC readiness
- Compliance-readiness limitations

## architecture/adr/

Every major architecture choice should have an ADR.

ADR format:

```markdown
# ADR-000X: Decision Title

## Status

Accepted

## Context

Why this decision is needed.

## Decision

What decision was made.

## Consequences

Positive and negative consequences.

## Alternatives Considered

Other options considered and why they were not selected.
```

---

# 19. Testing Standards

Testing must be included from the beginning.

## Backend Testing

Expected test types:

- Unit tests
- Application service tests
- Domain rule tests
- API integration tests where practical
- Validation tests
- Repository abstraction tests
- Mock clinical AI provider tests
- Cosmos document mapping tests where practical

Important scenarios:

- Patient registration
- Provider creation
- Appointment scheduling
- Invalid appointment status transition
- Care plan creation
- Care plan task status update
- Clinical note insight generation with mock provider
- Azure AI provider configuration missing safe error
- Notification request creation
- Audit event creation
- Correlation ID propagation
- Safe logging expectations

## Frontend Testing

Expected test types:

- Component tests where practical
- Hook tests where practical
- Form validation tests
- API service tests
- Routing smoke tests where practical

## Test Data

Use synthetic data only.

No real patient data.

No real clinical notes.

---

# 20. CI/CD Standards

GitHub Actions should be added early.

Minimum CI workflow:

```text
on:
  pull_request
  push to main
```

Pipeline should:

- Restore backend dependencies
- Build backend
- Run backend tests
- Install frontend dependencies
- Build React frontend
- Run frontend tests where practical
- Validate Docker builds where practical

Future CD workflow should:

- Build Docker images
- Push images to Azure Container Registry
- Deploy backend to Azure Container Apps
- Deploy frontend to Azure Static Web Apps
- Configure Azure SQL, Cosmos DB, Key Vault, and Application Insights through IaC

CI must not require Azure AI credentials.

Local and CI default must be:

```text
AI_PROVIDER=Mock
```

---

# 21. Agent Workflow Rules

When receiving an epic-based prompt, the agent must follow this workflow.

## Step 1: Understand the Epic

Read:

- Existing code
- README
- AGENTS.md
- Relevant architecture docs
- Existing tests
- Existing workflows

Do not assume the repository is empty.

## Step 2: Create an Implementation Plan

Before coding, identify:

- Files to create
- Files to modify
- Architecture impact
- Database impact
- Privacy impact
- AI impact
- Testing impact
- Documentation impact

## Step 3: Implement Incrementally

Make small, coherent changes.

Avoid massive unstructured changes.

Keep code buildable.

Do not leave half-implemented features.

## Step 4: Add or Update Tests

Every meaningful feature must include tests.

If tests cannot be added, explain why in final summary.

## Step 5: Update Documentation

Update relevant docs after implementation.

At minimum update:

- README.md when user-facing behavior changes
- architecture docs when architecture changes
- docs/roadmap.md after completing an epic
- privacy/responsible AI docs when relevant

## Step 6: Validate

Before finishing, run or provide commands to run:

- Backend build
- Backend tests
- Frontend build
- Frontend tests
- Docker Compose validation where applicable

## Step 7: Summarize

Final response should include:

- What was implemented
- Key files changed
- How to run
- How to test
- Architecture notes
- Privacy and responsible AI notes
- Next recommended epic

---

# 22. Agent Response Format

For every implementation epic, the agent should respond using this format:

```markdown
## Completed

Summary of what was implemented.

## Architecture Notes

Important architecture decisions or changes.

## Privacy and Responsible AI Notes

Healthcare privacy, safe logging, synthetic data, and AI safety considerations.

## Files Changed

List of important files added or modified.

## How to Run

Commands to run the application.

## How to Test

Commands to run tests.

## Documentation Updated

Docs that were created or updated.

## Suggested Next Epic

Recommended next step.
```

---

# 23. MVP Epic Roadmap

The user may give short epic prompts. The agent must infer technical tasks from this roadmap.

## Epic 0: Repository Foundation

Goal:

Create the initial enterprise-grade repository foundation.

Expected output:

- Solution structure
- .NET backend skeleton
- React frontend skeleton
- SQL Server and Cosmos DB readiness
- Mock AI provider readiness
- Docker Compose skeleton
- README
- AGENTS.md
- Architecture docs
- Privacy and responsible AI docs
- ADRs
- Initial CI workflow

## Epic 1: Patient Registration

Goal:

Build patient registration backend and React frontend flow.

Expected output:

- Patient API
- Patient entity
- SQL Server persistence
- Validation
- React patient form
- Patient list/details
- Tests
- Documentation update

## Epic 2: Provider Management

Goal:

Build provider profile management.

Expected output:

- Provider API
- Provider entity
- SQL Server persistence
- Validation
- React provider screens
- Tests
- Documentation update

## Epic 3: Appointment Scheduling

Goal:

Schedule appointments between patients and providers.

Expected output:

- Appointment API
- Appointment entity
- SQL Server persistence
- Appointment status model
- React scheduling screens
- Tests
- Documentation update

## Epic 4: Care Plan Management with Cosmos DB

Goal:

Create and manage patient care plans using Cosmos DB document model.

Expected output:

- CarePlan API
- Care plan document model
- Cosmos DB repository abstraction
- Mock/local Cosmos strategy if needed
- React care plan screens
- Tests
- Polyglot persistence documentation update

## Epic 5: Clinical Note Insights with Mock AI Provider

Goal:

Analyze synthetic clinical notes using a mock clinical text analyzer.

Expected output:

- ClinicalInsights API
- MockClinicalTextAnalyzer
- Clinical insight document model
- Cosmos DB persistence
- React clinical insights screens
- Responsible AI notice
- Tests
- Documentation update

## Epic 6: Azure AI Language Provider Readiness

Goal:

Add optional Azure AI Language Text Analytics for Health provider readiness.

Expected output:

- AzureTextAnalyticsForHealthProvider
- Provider abstraction
- Environment-based configuration
- Key Vault readiness documentation
- Mock remains default locally
- Tests without real Azure credentials
- Documentation update

## Epic 7: Follow-up Task Tracking

Goal:

Track follow-up tasks under care plans.

Expected output:

- Task model
- Task status transitions
- React task views
- Cosmos DB persistence
- Tests
- Documentation update

## Epic 8: Notification Simulation

Goal:

Create notification request and simulated delivery workflow.

Expected output:

- Notification worker/API
- Notification event model
- Email/SMS/portal simulation
- Cosmos DB persistence
- React notification history
- Tests
- Documentation update

## Epic 9: Audit Logging with Cosmos DB

Goal:

Add centralized audit logging and traceability.

Expected output:

- Audit API
- Audit event document model
- Cosmos DB persistence
- Audit query APIs
- React audit trail screens
- Tests
- Documentation update

## Epic 10: Security, Privacy, and RBAC Readiness

Goal:

Add security and privacy readiness patterns.

Expected output:

- RBAC readiness documentation
- Role model readiness
- Safe logging review
- Secure headers where practical
- Privacy and compliance docs
- Tests where practical

## Epic 11: Observability and Production Readiness

Goal:

Add production-readiness patterns.

Expected output:

- Correlation ID
- Structured logging
- Health checks
- Error handling middleware
- Application Insights readiness
- Operational runbook

## Epic 12: DevOps and Docker

Goal:

Make project easy to build and run.

Expected output:

- Dockerfiles
- Docker Compose
- GitHub Actions CI
- Build validation
- Test validation
- Developer setup documentation

## Epic 13: Azure Deployment Blueprint

Goal:

Prepare Azure deployment architecture.

Expected output:

- Bicep templates
- Azure resource plan
- Azure SQL
- Cosmos DB
- Azure AI Language
- Key Vault
- Application Insights
- Deployment documentation

## Epic 14: Portfolio Polish

Goal:

Make repository impressive for GitHub visitors.

Expected output:

- Professional README
- Architecture diagrams
- Screenshots or placeholders
- Roadmap
- Privacy and responsible AI section
- Final documentation review

---

# 24. Default Behavior for Short User Prompts

The user may provide prompts like:

```text
Implement Epic 1.
```

or:

```text
Add care plan feature.
```

or:

```text
Make clinical insights Azure AI ready.
```

When prompts are short, the agent must not ask for unnecessary technical clarification.

The agent should use this AGENTS.md file as the source of technical direction.

The agent should proceed with best-practice implementation based on:

- Current repository state
- Project roadmap
- Healthcare architecture expectations
- Privacy and responsible AI expectations
- Polyglot persistence direction
- Azure deployment direction
- Existing code and documentation

Ask clarification only when a decision would significantly change architecture, cost, security, privacy, AI provider behavior, or user-facing behavior.

---

# 25. Quality Bar

A feature is not complete unless:

- Code builds successfully
- Relevant tests are added or updated
- API behavior is documented
- React frontend behavior is usable where applicable
- Validation and error handling are implemented
- SQL Server or Cosmos DB persistence is appropriate for the module
- Privacy and safe logging concerns are addressed
- Responsible AI concerns are addressed where applicable
- Architecture docs are updated if needed
- README or setup docs are updated if commands changed
- No secrets are committed
- No real patient data is used
- Azure-readiness is preserved

---

# 26. Definition of Done

For every epic, the Definition of Done is:

```text
1. Feature implemented end-to-end or clearly scoped for the epic.
2. Backend code follows clean architecture or clear service boundaries.
3. React code follows feature-based structure.
4. API contracts are documented through Swagger/OpenAPI.
5. Validation and error handling are implemented.
6. Tests are added or updated.
7. Logging and correlation ID impact are considered.
8. Privacy and safe data handling are considered.
9. Responsible AI concerns are addressed where applicable.
10. Documentation is updated.
11. Local run instructions are clear.
12. Azure-readiness is preserved.
13. No secrets, credentials, real patient data, or real clinical data are committed.
14. Final response explains what changed and how to validate it.
```

---

# 27. Non-Negotiable Rules

The agent must not:

- Build a toy-style healthcare CRUD app
- Claim HIPAA compliance
- Claim clinical certification
- Claim AI diagnosis
- Use real patient data
- Use real clinical notes
- Log full clinical notes
- Log secrets
- Hardcode Azure credentials
- Expose backend secrets to React frontend
- Require Azure AI credentials for local development
- Make Azure AI provider the default local provider
- Put all backend logic in controllers
- Put all frontend logic in React page components
- Skip validation
- Ignore tests
- Ignore documentation
- Ignore privacy and responsible AI notes
- Remove architecture documents without replacement
- Introduce Azure resources without documenting cost and purpose

---

# 28. Preferred Naming Conventions

## Backend

Use clear names:

- Patient
- Provider
- Appointment
- CarePlan
- FollowUpTask
- ClinicalNote
- ClinicalInsight
- ExtractedClinicalEntity
- NotificationRequest
- AuditEvent
- ConsentStatus
- AppointmentStatus
- CarePlanStatus
- TaskStatus
- MockClinicalTextAnalyzer
- AzureTextAnalyticsForHealthProvider

Avoid vague names:

- Data
- Info
- Helper
- Manager
- Processor
- Common
- Utility

## Frontend

Use clear feature naming:

- patient-registration
- patient-profile
- provider-management
- appointment-scheduling
- care-plan-workspace
- clinical-insights
- follow-up-tasks
- notification-history
- audit-trail
- system-health

---

# 29. Portfolio Presentation Expectations

This repository should visually communicate seniority.

README should eventually include:

- Project banner or title
- Architecture diagram
- Feature list
- Tech stack badges
- Local setup
- Screenshots
- SQL Server and Cosmos DB explanation
- Azure AI Language readiness
- Privacy and responsible AI section
- Azure deployment architecture
- CI/CD status badge
- Roadmap
- Architecture decision records
- Observability notes
- Security notes

The repository should make the visitor think:

```text
This person understands healthcare workflows, secure system design, polyglot persistence, Azure AI readiness, and enterprise-grade Solution Architecture.
```

---

# 30. Final Guidance for AI Agents

When implementing, always optimize for:

- Clear architecture
- Healthcare domain realism
- Professional repository presentation
- Maintainability
- Secure and private data handling
- Responsible AI design
- SQL Server and Cosmos DB fit-for-purpose usage
- Azure readiness
- Strong documentation
- Clean code
- Testability
- Recruiter/interviewer impact

This project should become a flagship GitHub portfolio project for a Solution Architect specializing in .NET, React, Azure, healthcare workflows, secure cloud-native architecture, polyglot persistence, and Azure AI-enabled healthcare insights.
