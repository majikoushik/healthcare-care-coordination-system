# ADR-0006: Mock AI Provider First

## Status

Accepted

## Context

Local development and CI must not require Azure credentials or process real clinical data.

## Decision

Use `MockClinicalTextAnalyzer` as the default local and CI provider.

## Consequences

Clinical insight behavior is deterministic and safe for tests. Real Azure integration is deferred.

## Alternatives Considered

Calling Azure AI directly in MVP setup was rejected because it would introduce credentials and cost too early.
