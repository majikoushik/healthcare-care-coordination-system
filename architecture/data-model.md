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

### Provider
- `Id` (Guid, PK)
- `FullName` (string, max 200)
- `Specialty` (enum: GeneralMedicine, Cardiology, Endocrinology, Orthopedics, Pediatrics, Neurology, Dermatology)
- `Email` (string, max 150)
- `MobileNumber` (string, max 50)
- `Department` (string, max 200)
- `AvailabilityStatus` (enum: Available, Busy, OnLeave, Inactive)
- `CreatedAt` (datetimeoffset)
- `UpdatedAt` (datetimeoffset, null)

### Appointment
- `Id` (Guid, PK)
- `PatientId` (Guid, FK conceptually)
- `ProviderId` (Guid, FK conceptually)
- `AppointmentDateTime` (datetimeoffset)
- `VisitReason` (string, max 500)
- `Type` (enum: Consultation, FollowUp, LabReview, MedicationReview, CarePlanReview)
-| Domain | Storage | Persistence Strategy |
|---|---|---|
| **Patient** | SQL Server | Relational tables (`Patients`) |
| **Provider** | SQL Server | Relational tables (`Providers`) |
| **Appointment** | SQL Server | Relational tables (`Appointments`) |
| **Care Plan** | Azure Cosmos DB | JSON Documents (Partition Key: `/patientId`) |
| **Follow-up Task** | Azure Cosmos DB | JSON Documents (Partition Key: `/patientId`) |
| **Clinical Insights** | Azure Cosmos DB | JSON Documents (Partition Key: `/patientId`) |
| **Notification Record** | Azure Cosmos DB | JSON Documents (Partition Key: `/patientId`) |
- `Status` (enum: Requested, Scheduled, CheckedIn, Completed, Cancelled, NoShow)
- `Notes` (string, max 2000)
- `CreatedAt` (datetimeoffset)
- `UpdatedAt` (datetimeoffset, null)

## Document Data (Azure Cosmos DB)

### CarePlan
**Partition Key:** `/patientId`

*Document Structure:*
```json
{
  "id": "guid",
  "patientId": "guid",
  "providerId": "guid",
  "relatedAppointmentId": "guid (optional)",
  "title": "string",
  "clinicalSummary": "string",
  "instructions": "string",
  "followUpDate": "datetimeoffset (optional)",
  "status": "enum: Draft, Active, OnHold, Completed, Cancelled",
  "goals": [
    {
      "goalId": "guid",
      "description": "string",
      "targetDate": "datetimeoffset (optional)",
      "status": "enum: NotStarted, InProgress, Achieved, NotAchieved, Cancelled",
      "priority": "enum: Low, Medium, High, Critical"
    }
  ],
  "tasks": [
    {
      "taskId": "guid",
      "taskTitle": "string",
      "taskDescription": "string",
      "dueDate": "datetimeoffset (optional)",
      "status": "enum: Pending, InProgress, Completed, Overdue, Cancelled",
      "priority": "enum: Low, Medium, High, Critical",
      "assignedTo": "string",
      "completedTimestamp": "datetimeoffset (optional)"
    }
  ],
  "createdAt": "datetimeoffset",
  "updatedAt": "datetimeoffset (optional)"
}
```

### ClinicalInsight
**Partition Key:** `/patientId`

*Document Structure:*
```json
{
  "id": "guid",
  "patientId": "guid",
  "providerId": "guid",
  "relatedCarePlanId": "guid (optional)",
  "relatedAppointmentId": "guid (optional)",
  "clinicalNoteText": "string",
  "extractedEntities": [
    {
      "entityId": "guid",
      "text": "string",
      "category": "enum: Symptom, Condition, Medication, Test, etc.",
      "confidenceScore": "double"
    }
  ],
  "suggestedFollowUpTopics": [ "string" ],
  "riskIndicators": [ "string" ],
  "aiProviderName": "string",
  "aiProviderMode": "enum: Mock, AzureAIReady",
  "humanReviewStatus": "enum: PendingReview, Reviewed, Approved, Rejected, RequiresClarification",
  "reviewedBy": "string (optional)",
  "reviewedTimestamp": "datetimeoffset (optional)",
  "createdAt": "datetimeoffset",
  "updatedAt": "datetimeoffset (optional)"
}
```

*(To be implemented in future epics: Audit Events)*
