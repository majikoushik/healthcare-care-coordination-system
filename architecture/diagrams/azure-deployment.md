# Azure Deployment

```mermaid
flowchart TB
  SWA[Azure Static Web Apps] --> ACA[Azure Container Apps]
  ACA --> AzureSql[(Azure SQL Database)]
  ACA --> Cosmos[(Azure Cosmos DB)]
  ACA --> Language[Azure AI Language]
  ACA --> ServiceBus[Azure Service Bus readiness]
  ACA --> KeyVault[Azure Key Vault]
  ACA --> AppInsights[Application Insights]
  AppInsights --> LogAnalytics[Log Analytics]
```
