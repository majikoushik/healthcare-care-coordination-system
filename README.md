# Healthcare Care Coordination System

Healthcare Care Coordination System is a cloud-native healthcare platform demo built with .NET, React, TypeScript, SQL Server, Azure Cosmos DB, and Azure AI Language-ready architecture. It demonstrates patient management, provider coordination, appointment scheduling, care plan workflows, clinical note insights, follow-up task tracking, notification simulation, audit logging, observability, and secure healthcare data handling patterns.

This repository is a Solution Architect portfolio project. It uses synthetic demo data only and is not intended for real clinical decision-making.

## Architecture Summary

- ASP.NET Core service boundaries for patient, provider, appointment, care plan, clinical insight, notification, and audit capabilities.
- React + TypeScript healthcare operations portal under `src/web/healthcare-care-portal`.
- SQL Server / Azure SQL direction for transactional master data.
- Azure Cosmos DB direction for care plans, clinical notes, insights, notifications, and audit events.
- Mock clinical AI provider for local development with Azure AI Language-ready provider abstraction.
- Correlation ID, health check, Problem Details, structured logging, and Application Insights readiness.

## Current Epic 1 Scope

Epic 1 implements the Patient Registration capability, including:
- React frontend forms and lists for Patient Management.
- .NET 8 minimal API backend with FluentValidation.
- Entity Framework Core with SQL Server for transactional persistence.
- Safe healthcare data handling and structured logging.

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

Docker Compose foundation:

```powershell
docker compose up --build
```

## Build And Test

```powershell
dotnet build src/services/Patient.Api/Patient.Api.csproj
dotnet test tests/ClinicalInsights.Api.Tests/ClinicalInsights.Api.Tests.csproj
cd src/web/healthcare-care-portal
npm install
npm run build
npm test
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
