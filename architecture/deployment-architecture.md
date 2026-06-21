# Deployment Architecture

The Healthcare Care Coordination System utilizes a cloud-native PaaS approach in Azure.

## Physical Architecture Components

1. **Frontend**: React SPA deployed on **Azure Static Web Apps**. It accesses the backend via a reverse proxy (in dev) or API Management/Direct Ingress in Azure.
2. **Backend**: .NET 8 Web APIs and Workers deployed on **Azure Container Apps**.
3. **Data Tier**:
   - **Azure SQL Database**: Stores relational data (Patients, Providers, Appointments).
   - **Azure Cosmos DB**: Stores NoSQL documents and events (Care Plans, Insights, Tasks, Notifications, Audit).
4. **Security**: **Azure Key Vault** securely stores all connection strings and keys. Accessed via User-Assigned **Managed Identity**.
5. **Observability**: **Application Insights** traces telemetry and pushes it to a **Log Analytics Workspace**.

## Diagram
Refer to the detailed physical layout in [Azure Deployment Diagram](diagrams/azure-deployment.md).
