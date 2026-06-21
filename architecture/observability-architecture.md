# Observability Architecture

This document defines the observability patterns used across the Healthcare Care Coordination System to ensure production readiness, troubleshooting capability, and system health monitoring.

## 1. Structured Logging
We use **Serilog** to provide structured logging capabilities.
All logs emitted by `ILogger<T>` in the application code are captured by Serilog and enriched with contextual metadata:
- `Service`: Identifies the microservice (e.g. `Patient.Api`).
- `Environment`: Environment executing the code.
- `CorrelationId`: The unique Trace ID linking cross-service requests.

### Privacy Rules
Logs **must never** contain:
- Protected Health Information (PHI).
- Raw Clinical AI inputs or outputs.
- Database Connection Strings or Secret Keys.

## 2. Distributed Tracing & Correlation
Every incoming HTTP request is inspected for an `X-Correlation-ID` header by the `CorrelationIdMiddleware`.
- If missing, a new Guid is generated.
- This ID is injected into the HTTP Response headers to allow clients (React Frontend) to log the ID for user-reported errors.
- The ID is pushed into the `ILogger` Scope, ensuring all log messages for the duration of that request share the same ID.

## 3. OpenTelemetry & Azure Monitor
The `Observability` building block includes readiness for OpenTelemetry and Application Insights.
- **Application Insights**: Can be activated instantly by providing an `ApplicationInsights:ConnectionString` in environment variables. This enables deep Application Map visualization and dependency tracking in Azure.
- **OpenTelemetry**: ASP.NET Core and HttpClient instrumentation is wired in via `AddOpenTelemetry()`, allowing future integration with Prometheus, Jaeger, or native Azure Monitor exporters.

## 4. Health Checks
Health Checks are mapped globally:
- `/health/live`: Lightweight check to ensure the API container is responsive. Used for Kubernetes Liveness Probes.
- `/health/ready`: Deep dependency check (SQL Server, Cosmos DB, AI). Used for Load Balancer traffic routing and the React System Health Dashboard.
