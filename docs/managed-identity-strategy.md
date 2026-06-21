# Managed Identity Strategy

To eliminate secret management overhead and reduce the attack surface, the system relies heavily on **Azure Managed Identities**.

## System-Assigned vs User-Assigned
- We utilize **User-Assigned Managed Identities** for the Azure Container Apps. This allows us to define the identity and its RBAC permissions in IaC (Bicep) before the Container Apps are provisioned, preventing circular dependencies.

## Key Integrations

### 1. Key Vault
The primary use case. Container Apps use their identity to read secrets from Key Vault via the `Key Vault Secrets User` role.

### 2. Azure Container Registry (ACR)
The Container Apps Environment uses Managed Identity with the `AcrPull` role to fetch images securely, bypassing the need to enable the Admin User on the registry.

### 3. Azure SQL & Cosmos DB
In future hardening epics, the APIs will connect to SQL and Cosmos DB via Microsoft Entra ID (using Managed Identity) instead of SQL admin passwords or Cosmos master keys. This Bicep blueprint establishes the identity foundation for this transition.
