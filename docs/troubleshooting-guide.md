# Troubleshooting Guide

Common issues encountered when running the `healthcare-care-coordination-system` locally.

## Docker Issues

### 1. Port Conflicts (5080-5085, 1433, 5173)
**Symptom:** `Error starting userland proxy: listen tcp4 0.0.0.0:5080: bind: address already in use`
**Fix:** Another service or previous docker container is using the port.
```powershell
docker compose down
# Check what is using the port
netstat -ano | findstr :5080
```

### 2. SQL Server Fails to Start
**Symptom:** The `sqlserver` container exits immediately with `AcceptEula error`.
**Fix:** Ensure your `.env` file explicitly has `ACCEPT_EULA=Y` or `docker-compose.yml` provides it.
**Symptom:** Bad password.
**Fix:** SQL Server 2022 enforces strong passwords. Ensure your `SQL_SERVER_PASSWORD` meets complexity requirements (Uppercase, Lowercase, Number, Symbol, >8 chars).

### 3. Build Caching Issues
**Symptom:** Docker container is not reflecting recent code changes.
**Fix:** Rebuild the images explicitly.
```powershell
docker compose up --build --force-recreate
```
Or use make:
```powershell
make down
make up
```

## Application Issues

### 1. AI Insights API Fails
**Symptom:** Calling the `clinical-insights-api` results in 500.
**Fix:** Ensure `CLINICAL_AI_PROVIDER_MODE=Mock` is set in your `.env`. If using Azure, ensure valid credentials are provided.

### 2. Care Plan API Fails
**Symptom:** Care plans cannot be saved, database connection refused.
**Fix:** Ensure `COSMOS_MODE=Mock` is set. The real emulator can be heavy and take minutes to boot. The mock mode bypasses real Cosmos DB locally.

### 3. Frontend Cannot Reach Backend
**Symptom:** Network errors in the React portal.
**Fix:** Ensure `VITE_*_API_URL` variables in `.env` match the backend microservice ports (default: `http://localhost:508X`). Note that if running locally without Docker, ports might differ slightly based on `launchSettings.json`.

## CI/CD Issues

### 1. GitHub Actions Fails on Tests
**Fix:** Run tests locally to reproduce.
```powershell
make test-backend
```
