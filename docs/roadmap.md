# Roadmap

## Completed MVP Capabilities

- Epic 0: Repository foundation, service skeletons, frontend shell, architecture docs, ADRs, Docker and CI foundations, and synthetic samples.
- Epic 1: Patient Registration with SQL Server persistence direction, validation, API contract, React patient screens, tests, and documentation.
- Epic 2: Provider Management with specialty, department, availability status, React provider screens, and tests.
- Epic 3: Appointment Scheduling with patient-provider association, scheduling status lifecycle, React appointment screens, and validation tests.
- Epic 4: Care Plan Management with Cosmos DB-ready document persistence, goals, instructions, and React care plan screens.
- Epic 5: Clinical Note Insights with `MockClinicalTextAnalyzer`, safe synthetic note handling, responsible AI notice, and React insight screens.
- Epic 6: Azure AI Language Provider Readiness with optional configuration, safe failure behavior, and local mock default.
- Epic 7: Follow-up Task Tracking with status, priority, due-date, overdue, and due-today workflow views.
- Epic 8: Notification Simulation for email, SMS, and portal notification history.
- Epic 9: Audit Logging with Cosmos DB-ready event documents and traceability queries.
- Epic 10: Security, Privacy, and RBAC Readiness with demo roles and Azure Entra ID direction.
- Epic 11: Observability and Production Readiness with correlation IDs, health endpoints, structured logging, and telemetry readiness.
- Epic 12: DevOps and Docker with Docker Compose, multi-stage Dockerfiles, CI, and setup docs.
- Epic 13: Azure Deployment Blueprint with Bicep, Key Vault, managed identity, Application Insights, Azure SQL, Cosmos DB, and Container Apps direction.
- Epic 14: Portfolio Polish with improved README, documentation alignment, UI wording, roadmap, and validation guidance.

## Future Improvements

- Real Azure AI Language integration in a controlled non-production environment.
- Azure Entra ID authentication and full JWT authorization policies.
- Azure API Management gateway.
- Real Azure Service Bus eventing.
- Advanced role-based workflows for care coordinators, providers, auditors, and administrators.
- Production deployment pipeline with environment approvals.
- Application Insights dashboards and alert rules.
- Automated integration testing.
- Synthetic E2E test automation.
- Advanced accessibility improvements.

## Boundaries

This repository remains a portfolio healthcare platform demo. It uses synthetic demo data only, demonstrates compliance-readiness patterns, and is not intended for real clinical decision-making.
