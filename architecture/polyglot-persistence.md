# Polyglot Persistence

SQL Server / Azure SQL owns structured transactional data for Patient, Provider, and Appointment modules.

Azure Cosmos DB owns flexible healthcare coordination documents and event-style histories for Care Plans, Clinical Insights, Notifications, and Audit Events.

Transaction boundaries stay inside the owning service. Cross-service consistency will use event-driven patterns and correlation IDs in later epics.
