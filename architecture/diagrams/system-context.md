# System Context

```mermaid
flowchart LR
  Coordinator[Care Coordinator] --> Portal[React Care Portal]
  Provider[Provider] --> Portal
  Portal --> APIs[Domain APIs]
  APIs --> Sql[(SQL Server / Azure SQL)]
  APIs --> Cosmos[(Azure Cosmos DB)]
  APIs --> Ai[Mock AI / Azure AI Language-ready]
  APIs --> Obs[Application Insights-ready Telemetry]
```
