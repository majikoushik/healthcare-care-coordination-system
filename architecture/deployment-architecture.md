# Deployment Architecture

Target Azure architecture:

- React portal: Azure Static Web Apps.
- APIs and workers: Azure Container Apps.
- Images: Azure Container Registry.
- Transactional data: Azure SQL Database.
- Document/event data: Azure Cosmos DB for NoSQL.
- Clinical insight readiness: Azure AI Language.
- Secrets: Azure Key Vault with managed identity.
- Messaging readiness: Azure Service Bus.
- Monitoring: Application Insights, Log Analytics, Azure Monitor.

Bicep is the preferred infrastructure-as-code direction.
