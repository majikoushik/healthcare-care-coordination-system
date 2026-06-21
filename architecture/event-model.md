# Event Model

Events represent business facts that already happened.

Initial event names:

- PatientRegistered
- ProviderRegistered
- AppointmentScheduled
- AppointmentStatusChanged
- CarePlanCreated
- CarePlanUpdated
- CarePlanTaskCompleted
- ClinicalInsightGenerated
- NotificationRequested
- NotificationDeliveryCompleted
- AuditEventRecorded

Payloads include event ID, event type, occurred timestamp, correlation ID, entity type, entity ID, patient ID when safe, and minimal business metadata. Full clinical notes are excluded.
