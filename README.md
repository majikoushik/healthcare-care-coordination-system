# Healthcare Care Coordination System

Healthcare Care Coordination System is a cloud-native healthcare platform demo built with .NET, React, TypeScript, SQL Server, Azure Cosmos DB, and Azure AI Language-ready architecture. It demonstrates patient management, provider coordination, appointment scheduling, care plan workflows, clinical note insights, follow-up task tracking, notification simulation, audit logging, observability, and secure healthcare data handling patterns.

This repository is a Solution Architect portfolio project. It uses synthetic demo data only and is not intended for real clinical decision-making.

## Architecture Summary

- ASP.NET Core service boundaries for patient, provider, appointment, care plan, clinical insight, notification, and audit capabilities.
- React + TypeScript healthcare operations portal under `src/web/healthcare-care-portal`.
- SQL Server / Azure SQL direction for transactional master data.
- Azure Cosmos DB direction for care plans, clinical notes, insights, notifications, and audit events.
- Mock clinical AI provider for local development with Azure AI Language-ready provider abstraction.
- Correlation ID, health check, Problem Details, Serilog structured logging, and Application Insights readiness.
- OpenTelemetry tracing and metrics pipeline wired up across all services.
- Demo RBAC with `X-Demo-User-Role` header authentication for portfolio review.

## Epic Coverage

| Epic | Feature |
|------|---------|
| 0 | Foundation (CI, Docker, building blocks) |
| 1 | Patient Registration & Management |
| 2 | Provider Management |
| 3 | Appointment Scheduling |
| 4 | Care Plan Management (Cosmos DB) |
| 5 | Clinical Note Insights (Mock AI) |
| 6 | Azure AI Language Provider Readiness |
| 7 | Follow-up Task Tracking |
| 8 | Notification Simulation |
| 9 | Audit Logging (Cosmos DB) |
| 10 | Security, Privacy & RBAC Readiness |
| 11 | Observability & Production Readiness |

## System Health & Observability (Epic 11)

Each backend service exposes three health endpoints:

- `GET /health/live` — Liveness: confirms the process is running (no DB check).
- `GET /health/ready` — Readiness: includes SQL Server / Cosmos DB connectivity checks.
- `GET /health` — Alias for readiness, backwards-compatible.

All services emit structured logs via **Serilog**, enriched with `CorrelationId`, `Service`, and `Environment`. The `X-Correlation-ID` header is propagated end-to-end across backend services and reflected back in HTTP responses. **Application Insights** and **OpenTelemetry** are configured via safe placeholders — set `ApplicationInsights:ConnectionString` in environment variables to activate.

The React portal's `/system-health` route displays a live operational dashboard polling all 7 service `/health/ready` endpoints.

## Run Locally

Frontend:

```powershell
cd src/web/healthcare-care-portal
npm install
npm run dev
```

Backend service example, after installing .NET 8 SDK:

```powershell
dotnet run --project src/services/Patient.Api/Patient.Api.csproj
```

Verify health endpoints:

```powershell
curl http://localhost:5080/health/live
curl http://localhost:5080/health/ready
```

Docker Compose foundation:

```powershell
docker compose up --build
```

## Build And Test

```powershell
dotnet build src/services/Patient.Api/Patient.Api.csproj
dotnet test tests/ClinicalInsights.Api.Tests/ClinicalInsights.Api.Tests.csproj
dotnet test tests/Observability.Tests/Observability.Tests.csproj
cd src/web/healthcare-care-portal
npm install
npm run build
```

## Privacy And Responsible AI

- Synthetic demo data only.
- Do not commit real patient data, real clinical notes, credentials, or secrets.
- Do not log full clinical notes or sensitive health details.
- The project demonstrates compliance-readiness patterns, not certified compliance.
- Clinical insight output is assistive demo output requiring qualified healthcare professional review.

## Azure Direction

Target Azure architecture uses Azure Static Web Apps, Azure Container Apps, Azure SQL Database, Azure Cosmos DB for NoSQL, Azure AI Language, Azure Service Bus readiness, Azure Key Vault, Application Insights, Log Analytics, and Bicep.

## Documentation

Start with [architecture/README.md](architecture/README.md), [docs/setup.md](docs/setup.md), and [docs/roadmap.md](docs/roadmap.md).

Key operational docs:
- [Operational Runbook](docs/operational-runbook.md)
- [Monitoring & Alerting](docs/monitoring-and-alerting.md)
- [Production Readiness Checklist](docs/production-readiness-checklist.md)
- [Observability Architecture](architecture/observability-architecture.md)
