# Data Model

This document describes the high-level data models used across the system, adhering to polyglot persistence principles.

## Transactional Data (SQL Server)

### Patient
- `Id` (Guid, PK)
- `FullName` (string, max 200)
- `DateOfBirth` (datetime)
- `Gender` (enum)
- `Email` (string, max 150)
- `MobileNumber` (string, max 50)
- `Address` (string, max 500)
- `EmergencyContactName` (string, max 200)
- `EmergencyContactNumber` (string, max 50)
- `ConsentStatus` (enum: NotProvided, Provided, Withdrawn)
- `CreatedAt` (datetimeoffset)
- `UpdatedAt` (datetimeoffset, null)

## Document Data (Azure Cosmos DB)

*(To be implemented in future epics: Care Plans, Clinical Notes, Audit Events)*
