# Setup

## Purpose

This guide gets the portfolio demo running locally without Azure credentials. Local development uses SQL Server through Docker Compose, mock Cosmos-style repositories where configured, and the `MockClinicalTextAnalyzer` for clinical insight analysis.

## Prerequisites

- .NET 10 SDK
- Node.js 20 or later
- npm
- Docker Desktop

## Configuration

Copy the placeholder environment file and keep local secrets out of Git:

```powershell
Copy-Item .env.example .env
```

Required local defaults:

- `AI_PROVIDER=Mock`
- `CLINICAL_AI_PROVIDER_MODE=Mock`
- `COSMOS_MODE=Mock`
- `Security__Mode=Demo`

## Docker Compose

```powershell
docker compose up --build -d
docker compose ps
```

Portal: `http://localhost:5173`

Service Swagger endpoints:

- Patient API: `http://localhost:5080/swagger`
- Provider API: `http://localhost:5081/swagger`
- Appointment API: `http://localhost:5082/swagger`
- Care Plan API: `http://localhost:5083/swagger`
- Clinical Insights API: `http://localhost:5084/swagger`
- Audit API: `http://localhost:5085/swagger`

## Frontend Only

```powershell
cd src/web/healthcare-care-portal
npm install
npm run dev
```

## Single Backend Service

```powershell
dotnet run --project src/services/Patient.Api/Patient.Api.csproj
```

Repeat with another service project when testing an individual boundary.

## Validation

```powershell
Get-ChildItem -Recurse -Filter *.csproj src | ForEach-Object { dotnet build $_.FullName }
Get-ChildItem -Recurse -Filter *.csproj tests | ForEach-Object { dotnet test $_.FullName }
cd src/web/healthcare-care-portal
npm ci
npm run build
npm test
```

## Privacy Note

Use synthetic demo data only. Do not paste real patient data, real provider data, real clinical notes, credentials, tokens, or production connection strings into this repository or local sample files.
