# Local Development

Local development is designed to work without Azure credentials.

- Backend APIs expose `/health` and `/api/v1/*/readiness` endpoints.
- Frontend uses `VITE_API_BASE_URL` with a local default.
- SQL Server is represented in Docker Compose as the transactional store.
- Cosmos DB emulator readiness is documented as a placeholder for later epics.
- Clinical AI uses `MockClinicalTextAnalyzer`.

Do not commit `.env` files or real connection strings.
