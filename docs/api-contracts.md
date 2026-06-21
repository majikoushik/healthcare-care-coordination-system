# API Contracts

All APIs use `/api/v1` routes, correlation ID propagation, health checks, and Problem Details error responses.

Epic 0 readiness endpoints:

- `GET /api/v1/patients/readiness`
- `GET /api/v1/providers/readiness`
- `GET /api/v1/appointments/readiness`
- `GET /api/v1/care-plans/readiness`
- `GET /api/v1/clinical-insights/readiness`
- `GET /api/v1/audit/readiness`

Business endpoints will be added in later epics and documented here.
