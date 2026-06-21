# High-Level Design

## Business Context

The platform demonstrates a care coordination journey across patients, providers, appointments, care plans, follow-up tasks, clinical notes, notifications, and audit trails.

## System Context

Users interact with a React care coordination portal. The portal calls domain-oriented ASP.NET Core APIs. Transactional data is owned by SQL Server / Azure SQL services, while flexible documents and events are owned by Azure Cosmos DB-ready services.

## Containers

- React portal: healthcare operations UI.
- Patient.Api, Provider.Api, Appointment.Api: transactional service boundaries.
- CarePlan.Api, ClinicalInsights.Api, Audit.Api: Cosmos DB document/event boundaries.
- Notification.Worker: future asynchronous notification simulation.
- Shared building blocks: observability, security, compliance, messaging, Cosmos, and clinical AI abstractions.

## Quality Attributes

Privacy by design, traceability, modularity, testability, Azure readiness, local developer usability, and honest responsible AI boundaries.
