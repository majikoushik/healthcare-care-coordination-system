# ADR-0001: Domain-Oriented Service Boundaries

## Status

Accepted

## Context

The repository must demonstrate enterprise healthcare architecture without becoming a monolithic CRUD sample.

## Decision

Use separate ASP.NET Core service boundaries for patient, provider, appointment, care plan, clinical insight, notification, and audit capabilities, supported by shared building blocks.

## Consequences

The structure is more explicit and portfolio-ready. Local orchestration needs Docker Compose and clear documentation.

## Alternatives Considered

A single API was simpler but would weaken service ownership and architecture signaling.
