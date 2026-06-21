# Setup

## Prerequisites

- .NET 8 SDK
- Node.js 20 or later
- npm
- Docker Desktop for SQL Server and future container orchestration

## Backend

```powershell
dotnet restore src/services/Patient.Api/Patient.Api.csproj
dotnet run --project src/services/Patient.Api/Patient.Api.csproj
```

Repeat for other service projects as needed.

## Frontend

```powershell
cd src/web/healthcare-care-portal
npm install
npm run dev
```

## Local AI Provider

Use `AI_PROVIDER=Mock`. Azure AI Language credentials are not required for local development.
