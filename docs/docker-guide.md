# Docker Guide

This document describes how Docker is used in the `healthcare-care-coordination-system`.

## Overview

We containerize our applications to ensure consistent environments across development, testing, and production.
- **Backend APIs (.NET)**: Run as non-root users (`appuser`) using a multi-stage Dockerfile that caches NuGet packages and publishes a lean runtime image.
- **Frontend Portal (React/Vite)**: Built using Node.js and served as static HTML/JS via an Nginx container using a multi-stage Dockerfile.
- **Databases**: Local development uses SQL Server Linux container (`mcr.microsoft.com/mssql/server:2022-latest`). Cosmos DB is handled either via emulator placeholder or mock abstractions (recommended locally).

## Docker Compose Configuration

The `docker-compose.yml` file is the local orchestrator.

### Running the Stack

Make sure you have an `.env` file containing local dev secrets (copy `.env.example`).

```powershell
docker compose up --build -d
```

### Stopping the Stack

```powershell
docker compose down
```

### Services

| Service Name | Container Name | Port Mapping | Image / Build |
|---|---|---|---|
| `patient-api` | `patient-api` | 5080 | Custom .NET 10 Build |
| `provider-api` | `provider-api` | 5081 | Custom .NET 10 Build |
| `appointment-api` | `appointment-api` | 5082 | Custom .NET 10 Build |
| `care-plan-api` | `care-plan-api` | 5083 | Custom .NET 10 Build |
| `clinical-insights-api` | `clinical-insights-api` | 5084 | Custom .NET 10 Build |
| `audit-api` | `audit-api` | 5085 | Custom .NET 10 Build |
| `notification-worker`| `notification-worker` | N/A | Custom .NET 10 Build |
| `portal` | `healthcare-care-portal` | 5173 -> 80 | Nginx Alpine / React |
| `sqlserver` | `sqlserver` | 1433 | SQL Server 2022 |

## Multi-stage Builds

### .NET APIs
The `src/services/Dockerfile` uses `mcr.microsoft.com/dotnet/sdk:10.0` for the build stage and `mcr.microsoft.com/dotnet/aspnet:10.0` for the runtime stage.
To improve security, the process runs as a dynamically added `appuser`.

### React Portal
The `src/web/healthcare-care-portal/Dockerfile` uses `node:22-alpine` for the build stage and `nginx:alpine` to serve the resulting `dist/` directory. Port 80 is exposed.

## Mock Dependencies
To avoid hard-dependency on heavy emulators or real Azure cloud instances during local dev, we default environment configurations (`.env`) to mock modes:
- `AI_PROVIDER=Mock`
- `COSMOS_MODE=Mock`
- `CLINICAL_AI_PROVIDER_MODE=Mock`

You can test full Cosmos local integration by turning on the `cosmos-emulator-placeholder` profile and replacing the connection strings, but `Mock` is recommended for general dev.
