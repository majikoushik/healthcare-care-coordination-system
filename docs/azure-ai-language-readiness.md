# Azure AI Language Readiness

The Clinical Insights module is Azure AI Language-ready but does not call Azure in Epic 0.

Provider direction:

- `MockClinicalTextAnalyzer`: local and CI default.
- `AzureTextAnalyticsForHealthProvider`: future optional provider.

Suggested environment variables:

```text
AI_PROVIDER=Mock
AZURE_AI_LANGUAGE_ENDPOINT=
AZURE_AI_LANGUAGE_KEY=
AZURE_AI_LANGUAGE_API_VERSION=
AZURE_AI_LANGUAGE_TIMEOUT_SECONDS=60
```

Credentials should come from user secrets locally or Azure Key Vault in deployed environments.
