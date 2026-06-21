# Architecture Documentation

## Purpose

This folder records the architecture for the Healthcare Care Coordination System portfolio demo. It is intended to show solution architecture thinking across service boundaries, data ownership, responsible AI, privacy, observability, DevOps, and Azure deployment readiness.

## Scope

The architecture covers an MVP healthcare coordination workflow:

- Patient and provider profile management.
- Appointment scheduling.
- Care plans and follow-up tasks.
- Synthetic clinical note insight analysis.
- Notification simulation.
- Audit logging and traceability.
- Demo RBAC and future Azure Entra ID direction.
- Local Docker development and Azure deployment blueprint.

## Key Views

- [High-Level Design](hld.md)
- [Low-Level Design](lld.md)
- [API Governance](api-governance.md)
- [Data Model](data-model.md)
- [Event Model](event-model.md)
- [Polyglot Persistence](polyglot-persistence.md)
- [Polyglot Persistence Strategy](polyglot-persistence-strategy.md)
- [Clinical AI Architecture](clinical-ai-architecture.md)
- [Clinical Insights AI Strategy](clinical-insights-ai-strategy.md)
- [Responsible AI Architecture](responsible-ai-architecture.md)
- [Security Architecture](security-architecture.md)
- [Privacy and Compliance-Readiness](privacy-and-compliance.md)
- [Observability Architecture](observability-architecture.md)
- [Deployment Architecture](deployment-architecture.md)
- [Non-Functional Requirements](nfrs.md)
- [ADRs](adr/)

## Diagram Index

- [System Context](diagrams/system-context.md)
- [Container Diagram](diagrams/container-diagram.md)
- [Patient Appointment Flow](diagrams/patient-appointment-flow.md)
- [Care Plan Flow](diagrams/care-plan-flow.md)
- [Clinical Insights Flow](diagrams/clinical-insights-flow.md)
- [Polyglot Persistence](diagrams/polyglot-persistence.md)
- [Audit Logging Flow](diagrams/audit-logging-flow.md)
- [Security and RBAC Flow](diagrams/security-and-rbac-flow.md)
- [Observability Flow](diagrams/observability-flow.md)
- [Correlation ID Flow](diagrams/correlation-id-flow.md)
- [Health Check Flow](diagrams/health-check-flow.md)
- [Azure Deployment](diagrams/azure-deployment.md)
- [Azure Data Flow](diagrams/azure-data-flow.md)
- [Azure CI/CD Flow](diagrams/azure-ci-cd-flow.md)

## Privacy And AI Boundaries

The system is designed for synthetic demo data only. It demonstrates compliance-readiness patterns, responsible AI guardrails, and human-review-first clinical insight workflows. It does not claim HIPAA compliance, clinical certification, diagnosis, medical advice, or real clinical decision support.
