# Data Model

## SQL Server / Azure SQL

- Patients: profile, contact, emergency contact, consent status, timestamps.
- Providers: specialty, department, availability status, timestamps.
- Appointments: patient-provider association, scheduled time, status, timestamps.

## Azure Cosmos DB

- CarePlans partitioned by `/patientId`.
- ClinicalInsights partitioned by `/patientId`.
- Notifications partitioned by `/entityId` or `/patientId`.
- AuditEvents partitioned by `/correlationId` initially, with patient query design reviewed in audit epic.

All examples are synthetic and must not represent real patient data.
