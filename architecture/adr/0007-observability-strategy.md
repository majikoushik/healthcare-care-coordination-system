# ADR-0007: Observability Strategy

## Status

Accepted

## Context

Healthcare workflows need traceability across APIs, workers, persistence, and future event flows.

## Decision

Start with correlation ID middleware, health checks, Problem Details readiness, structured logging readiness, and Application Insights direction.

## Consequences

Future features inherit traceability expectations from the foundation.

## Alternatives Considered

Deferring observability was rejected because traceability is part of the project's core architecture story.
