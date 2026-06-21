# Azure AI Language Readiness

The Healthcare Care Coordination System demonstrates enterprise architecture readiness for advanced cognitive services. Specifically, the Clinical Insights module is built to seamlessly integrate with **Azure AI Language (Text Analytics for Health)**.

## Architectural Goal
Real medical records contain unstructured text loaded with highly sensitive Protected Health Information (PHI). Directly extracting structured entities (like symptoms, diagnoses, or medication dosages) natively would be enormously complex and error-prone. By targeting Azure Text Analytics for Health, the platform leverages a mature, HIPAA-compliant (in Azure) cognitive service.

## Our Approach: "Safe by Default"

To guarantee that this repository never accidentally leaks real credentials or requires a heavy setup for local reviewers:
1. **Abstraction Boundary**: We interact entirely through the `IClinicalTextAnalyzer` interface.
2. **Local Mocking**: By default (`ProviderMode = Mock`), the application uses `MockClinicalTextAnalyzer`, avoiding any network calls.
3. **Configuration Driven**: Toggling to Azure is controlled by `appsettings.json` or Environment Variables without touching application code.

### Configuration

```json
"ClinicalAI": {
  "ProviderMode": "Mock", // Mock | AzureAIConfigured
  "AzureLanguage": {
    "Endpoint": "https://<your-resource>.cognitiveservices.azure.com/",
    "Key": "<from-key-vault>",
    "Region": "eastus",
    "ModelVersion": "latest"
  }
}
```

## Security & Secrets
- **NEVER** hardcode Azure Language keys into `appsettings.json` or source control.
- In production, rely on **Azure Managed Identity** (via `DefaultAzureCredential` from the Azure Identity SDK) instead of static keys.
- If keys must be used during development, utilize `.NET User Secrets` (`dotnet user-secrets set "ClinicalAI:AzureLanguage:Key" "..."`).

## Responsible AI Safeguards
Any insight derived from the AI Provider (Mock or Azure) is forcibly stamped with `HumanReviewStatus = PendingReview` and prominently displays a disclaimer on the React frontend. It cannot automatically influence appointment generation or care plan goals without explicit action from a Care Coordinator.
