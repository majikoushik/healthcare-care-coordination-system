# Clinical Insights Flow

```mermaid
sequenceDiagram
  participant Portal
  participant InsightsApi
  participant Analyzer as MockClinicalTextAnalyzer
  participant Cosmos
  Portal->>InsightsApi: Submit synthetic clinical note
  InsightsApi->>Analyzer: Analyze without logging full note
  Analyzer-->>InsightsApi: Extracted terms and review notice
  InsightsApi->>Cosmos: Store clinical insight document
```
