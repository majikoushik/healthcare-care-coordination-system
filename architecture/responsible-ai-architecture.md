# Responsible AI Architecture

Clinical insight features must remain assistive, synthetic-data-only, and review-oriented.

- Default provider is `MockClinicalTextAnalyzer`.
- Azure AI Language readiness is optional and configuration-driven.
- Outputs must use terms such as extracted entities, possible clinical terms, and suggestions for review.
- Outputs must not be presented as diagnosis, treatment advice, or clinical decision support.
- Full clinical notes must not be logged by default.
