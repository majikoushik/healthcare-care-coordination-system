# Privacy by Design

Because this application mimics a clinical setting, **Privacy by Design** principles are treated as a core architectural constraint, even though the application explicitly utilizes **only synthetic data**.

## Synthetic Data Warning
> **WARNING**: This repository contains synthetic, generated mock data. It does not reflect real patient PII, real medical records, or genuine clinical diagnoses. Do not inject real clinical payloads into this system.

## Data Classification Model

| Classification Level | Examples in the System | Handling Rules |
|---|---|---|
| **PublicDemo** | Project Description, Architecture Docs | Visible in GitHub. |
| **InternalDemo** | Synthetic Provider IDs, Care Plan Statuses | Safe to log for tracing. |
| **SensitiveHealthcareLike** | Patient Names, DOB, Clinical Notes, AI Insights | **NEVER** logged in plaintext. Masked in traces. |
| **RestrictedSecret** | Azure Keys, DB Connection Strings | Strictly stored in Azure Key Vault or User Secrets. |
| **AuditMetadata** | Correlation IDs, Actions, Emitter IDs | Sent to Audit Log as "Safe Metadata". No clinical values. |

## Responsible AI Safeguards (Clinical Insights)
The AI Integration within this platform implements responsible-AI circuit breakers:
1. **Asymmetric Processing**: AI parses notes, but does NOT trigger automatic patient-facing notifications.
2. **Mandatory Human-in-the-Loop**: All `ClinicalNoteInsight` documents start in a `PendingReview` state and require a user with `ClinicalInsight.Review` permission to approve them.
3. **Clinical Disclaimer**: AI output is explicitly labeled as *Assistive Analysis*, and UI components carry a `PrivacyNotice` banner reminding users not to use the platform for genuine medical diagnostics.
