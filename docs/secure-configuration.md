# Secure Configuration Guide

This repository contains health-related architectural concepts but uses synthetic demo data only. Even so, it demonstrates secure configuration and secret-handling enterprise patterns.

## Secret Management Principles
1. **Never Commit Secrets:**
   - `.env` files are excluded in `.gitignore`.
   - `.env.example` contains only fake placeholders and connection strings.
   - Do not hardcode passwords, Azure keys, or Connection Strings anywhere in code.

2. **Local Environment:**
   - Developers must manually copy `.env.example` to `.env` and fill in local passwords (e.g., `SQL_SERVER_PASSWORD`).
   - Mock providers (`AI_PROVIDER=Mock`, `COSMOS_MODE=Mock`) are the default to avoid Azure dependencies and potential credential leakage.

3. **CI/CD Security:**
   - GitHub Actions workflows use Mock modes.
   - No Azure credentials are required or provided in the CI pipeline.

4. **Production Readiness (Azure):**
   - For real Azure deployment, the application is designed to fetch secrets dynamically at runtime from **Azure Key Vault**.
   - Managed Identity (`DefaultAzureCredential`) is preferred over connection strings where possible in Azure services.

## Healthcare Privacy Alignment
- Logs do not print patient names or clinical note contents.
- Audit records store correlation IDs and event types, but abstract sensitive entity details.
- Database backups or dumps are explicitly prohibited from being committed to the repo.
