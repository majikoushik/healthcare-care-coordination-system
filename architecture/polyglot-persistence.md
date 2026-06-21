# Polyglot Persistence

### SQL Server / Azure SQL (Relational Store)
- **Patients**: Master data, structured properties.
- **Providers**: Master data, structured properties.
- **Appointments**: Master data, highly relational to Patients and Providers, scheduling integrity is critical.

### Azure Cosmos DB (Document Store)
Azure Cosmos DB is used for flexible, schemaless documents and event sourcing. It handles unstructured or semi-structured clinical documents perfectly. In local environments, this is mocked via `MockCarePlanRepository` to avoid needing active Azure credentials.

**Current Document Models:**
- **Care Plans**: A JSON document mapping the patient's care journey. Contains embedded arrays for dynamic `Goals` and `Tasks`. Partitioned by `/patientId` to colocate all care-related data for a patient in a single logical partition for incredibly fast retrieval.

**Future Document Models:**
- **Clinical Insights**: Extracted unstructured data from Azure AI Language models.
- **Audit Logging**: Append-only event history.
- **Notifications**: Append-only event triggers.

Transaction boundaries stay inside the owning service. Cross-service consistency will use event-driven patterns and correlation IDs in later epics.
