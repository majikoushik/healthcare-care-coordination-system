# Testing Strategy

## Purpose

Testing validates the portfolio demo's domain boundaries, safety constraints, and developer workflow without requiring Azure credentials or real healthcare data.

## Current Coverage

- Backend test projects exist for patient, provider, appointment, care plan, clinical insights, notification, audit, security, and observability boundaries.
- Security tests cover healthcare role and permission definitions.
- Observability tests cover correlation ID middleware behavior.
- Frontend smoke tests validate the React application shell.

## Backend Test Commands

```powershell
Get-ChildItem -Recurse -Filter *.csproj tests | ForEach-Object { dotnet test $_.FullName }
```

## Frontend Test Commands

```powershell
cd src/web/healthcare-care-portal
npm ci
npm test
npm run build
```

## CI Requirements

- CI must default to `AI_PROVIDER=Mock`.
- CI must not require Azure AI Language credentials.
- CI must not require real Cosmos DB keys, SQL production credentials, Key Vault access, or Application Insights secrets.

## Future Test Improvements

- API integration tests for each service boundary.
- Repository contract tests for Cosmos DB-ready modules.
- Synthetic E2E tests for patient to appointment to care plan workflows.
- Frontend form validation and route-level tests.
- Safe logging tests for clinical note and audit metadata behavior.
