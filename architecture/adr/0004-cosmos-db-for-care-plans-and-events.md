# ADR-0004: Cosmos DB For Care Plans And Events

## Status

Accepted

## Context

Care plans, insight outputs, notification histories, and audit trails vary in structure and query shape.

## Decision

Prepare Cosmos DB containers for CarePlans, ClinicalInsights, Notifications, and AuditEvents.

## Consequences

Partition key strategy must be documented and validated during each feature epic.

## Alternatives Considered

Storing all documents in relational JSON columns was rejected because it reduces document-model clarity.
