# Operational Runbook

Epic 0 operational checks:

- Confirm services expose `/health`.
- Confirm requests carry `X-Correlation-ID`.
- Confirm `AI_PROVIDER=Mock` in local and CI environments.
- Confirm no real patient data, clinical notes, secrets, or credentials are committed.
