# Local Development

## Purpose

Local development is designed to exercise the healthcare workflow without Azure credentials or real healthcare data.

## Runtime Defaults

- Frontend uses per-service `VITE_*_API_URL` variables.
- SQL Server runs through Docker Compose for transactional services.
- Cosmos DB-backed modules use mock/local repository abstractions unless explicitly configured otherwise.
- Clinical AI uses `MockClinicalTextAnalyzer`.
- Demo RBAC uses `X-Demo-User-Role` and the local role selector in the React portal.

## Health Checks

Each backend service exposes:

- `/health/live`
- `/health/ready`
- `/health`

The portal `/system-health` page polls service readiness endpoints and displays operational status for portfolio review.

## Safe Data Rules

Do not commit `.env` files, real connection strings, real patient data, real clinical notes, secrets, tokens, or Azure keys. Use `samples/` only for synthetic demo payloads.
