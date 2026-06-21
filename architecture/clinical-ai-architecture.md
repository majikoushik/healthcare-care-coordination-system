# Clinical AI Architecture

Clinical note insights use a provider abstraction.

- Local and CI default: `MockClinicalTextAnalyzer`.
- Future Azure-ready provider: `AzureTextAnalyticsForHealthProvider`.
- No Azure AI credentials are required in Epic 0.
- Full clinical notes must not be logged by default.
- Outputs are extracted terms and follow-up suggestions for review, not diagnosis or medical advice.

Human review notice:

```text
This clinical insight is AI-assisted demo output and must be reviewed by a qualified healthcare professional before any real clinical use.
```
