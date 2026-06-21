# ADR-0008: Azure Hosting Strategy

## Status

Accepted

## Context

The project should be cloud-native and Azure-ready while staying runnable locally.

## Decision

Target Azure Static Web Apps for the portal, Azure Container Apps for APIs/workers, Azure SQL, Azure Cosmos DB, Key Vault, Application Insights, Log Analytics, and Service Bus readiness.

## Consequences

The architecture aligns with modern Azure platform services and can evolve through Bicep.

## Alternatives Considered

Virtual machines were rejected because they add operational burden and weaken cloud-native positioning.
