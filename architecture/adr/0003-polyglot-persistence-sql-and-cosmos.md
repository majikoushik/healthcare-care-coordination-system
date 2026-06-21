# ADR-0003: Polyglot Persistence With SQL And Cosmos

## Status

Accepted

## Context

Healthcare workflows include structured master data and flexible coordination documents/events.

## Decision

Use SQL Server / Azure SQL for Patient, Provider, and Appointment data. Use Azure Cosmos DB for Care Plans, Clinical Insights, Notifications, and Audit Events.

## Consequences

Each workload uses a suitable data store. Cross-store consistency must be explicit and event-driven.

## Alternatives Considered

Using only SQL Server was simpler but less representative of cloud-native document/event workloads.
