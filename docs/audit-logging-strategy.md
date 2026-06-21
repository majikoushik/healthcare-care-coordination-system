# Audit Logging Strategy

## Overview

The `HealthcareCareCoordination` system implements an **append-only, Cosmos DB-ready** audit trail. This ensures that every significant domain action is recorded, traceable, and compliant with enterprise architecture expectations.

## Safe Metadata Pattern

A critical privacy pattern enforced in this repository is the concept of **"Safe Metadata"**. 
Audit logs **must never** contain:
- Sensitive clinical notes (e.g., patient symptoms, full diagnoses)
- Patient Personally Identifiable Information (PII) beyond referential IDs
- Plaintext secrets or credentials

Instead, the `metadata` property is reserved for structural identifiers, state transitions (e.g., `OldStatus` -> `NewStatus`), and non-sensitive attributes (e.g., `ConsentStatus` flags, `Channel` types).

## Architecture

We use a central **Audit.Api** to handle persistence.
Upstream services (like `Patient.Api` or `Appointment.Api`) do not write directly to the database. Instead, they utilize the `IAuditLogger` abstraction provided in the `SharedKernel`.

1. **`IAuditLogger`**: An interface injected into upstream application services.
2. **`HttpAuditLogger`**: The default implementation for MVP which sends a fire-and-forget REST request to `Audit.Api`.
3. **`Audit.Api`**: A minimal API which validates the payload and persists it to the `IAuditEventRepository`.
4. **`MockAuditEventRepository`**: The Cosmos DB emulator placeholder partitioning by `/correlationId`.

## Partition Strategy
Cosmos DB requires a logical partition key. We have selected `/correlationId` because audit events are frequently queried chronologically across a single distributed transaction. Secondary lookup by `/patientId` is achieved via cross-partition queries or future materialized views, depending on scale.

## Implementation Example
```csharp
await auditLogger.LogEventAsync(
    eventType: "PatientRegistered",
    entityType: "Patient",
    entityId: patient.Id.ToString(),
    action: "RegisterPatient",
    outcome: "Success",
    summary: "Synthetic patient profile was registered successfully.",
    metadata: new { patient.ConsentStatus },
    patientId: patient.Id.ToString(),
    cancellationToken: cancellationToken);
```
