# Production Readiness Checklist

Before transitioning from local "Demo Mode" to a true Production Azure Environment, the following checklist must be completed.

## 1. Security & Identity
- [ ] Swap `DemoAuthenticationHandler` for `AddJwtBearer()` in `SecurityExtensions.cs`.
- [ ] Configure Azure Entra ID App Registrations and assign roles.
- [ ] Move SQL Server connection strings to Azure Key Vault.
- [ ] Move Azure AI Language API keys to Azure Key Vault.

## 2. Observability
- [ ] Inject a real `ApplicationInsights:ConnectionString` via Azure Container Apps environment variables.
- [ ] Enable Azure Monitor Log Analytics workspace.
- [ ] Configure OpenTelemetry exporter for Prometheus metrics (if required by enterprise standards).

## 3. Data & Persistence
- [ ] Swap Cosmos DB Emulator for real Azure Cosmos DB NoSQL instances.
- [ ] Ensure Cosmos DB partitions are correctly sizing.
- [ ] Ensure SQL Server Azure SQL Databases are deployed and Entity Framework migrations are executed securely.

## 4. Privacy & Compliance
- [ ] Confirm no PHI is being logged out by `ILogger` or exceptions.
- [ ] Ensure the AI Provider Disclaimer is legally compliant for your region.

## 5. Operations
- [ ] Setup Uptime checks calling `/health/ready` on all microservices via Azure Front Door / App Gateway.
- [ ] Validate CI/CD GitHub Actions trigger deployment to Azure Container Registry.
