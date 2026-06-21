# Application Insights Readiness

## Purpose

This document explains how the portfolio demo is prepared for Azure Application Insights without requiring Azure credentials for local development or CI.

## Current MVP Implementation

- Backend services expose health endpoints for liveness and readiness.
- Correlation IDs are propagated through the `X-Correlation-ID` header.
- Structured logging is configured through shared observability building blocks.
- Application Insights configuration is placeholder-driven through environment variables.
- Local development works without an Application Insights connection string.

## Configuration

Local and CI default:

```text
APPLICATIONINSIGHTS_CONNECTION_STRING=
```

Azure deployment direction:

- Store the real connection string in Azure Key Vault.
- Grant backend Container Apps access through managed identity.
- Inject telemetry configuration at deployment time.
- Do not commit telemetry secrets to source control.

## Telemetry Guidance

Recommended telemetry dimensions:

- Service name
- Environment name
- Correlation ID
- Request path
- HTTP status code
- Domain event name where applicable
- Safe entity type and entity ID where appropriate

Do not log:

- Secrets
- Tokens
- Connection strings
- Full clinical notes
- Full patient addresses
- Sensitive notification bodies
- Raw Azure AI request or response payloads

## Production Considerations

Future production hardening should add dashboards, alert rules, dependency tracking, sampling strategy, and privacy review gates before sending telemetry from any real environment.

