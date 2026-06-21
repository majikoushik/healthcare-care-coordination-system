# Low-Level Design

## Service Internals

Each API starts with a minimal ASP.NET Core service shell, health endpoint, readiness endpoint, Problem Details configuration, and correlation ID middleware. Business workflows will be added in later epics behind feature-specific DTOs, validators, services, and repositories.

## API Flow

Requests receive or generate an `X-Correlation-ID`, execute through domain endpoints, and return consistent response envelopes or Problem Details errors.

## Persistence Flow

Patient, Provider, and Appointment modules will use SQL Server / Azure SQL. Care Plan, Clinical Insights, Notification, and Audit modules will use Cosmos DB-ready document abstractions.

## Error Handling

Global exception handling returns a safe Problem Details response without secrets, full clinical notes, or sensitive health details.
