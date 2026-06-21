# API Governance

APIs use `/api/v1` routes, request/response DTOs, validation, Problem Details errors, correlation ID propagation, health checks, and Swagger/OpenAPI readiness.

Standard success envelope:

```json
{
  "data": {},
  "correlationId": "string",
  "timestamp": "2026-01-01T10:00:00Z"
}
```

Error responses follow Problem Details and include a safe correlation ID.
