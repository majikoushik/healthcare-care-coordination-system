# Deployment Overview

This repository uses a formal Azure Deployment Blueprint (Epic 13) to manage infrastructure as code.

## Target Azure Architecture

- **Frontend**: Azure Static Web Apps for the React portal.
- **Backend APIs**: Azure Container Apps (serverless containers).
- **Image Registry**: Azure Container Registry.
- **Relational Data**: Azure SQL Database (Patient, Provider, Appointment).
- **Document/Event Data**: Azure Cosmos DB (Care Plans, Insights, Tasks, Notifications, Audit).
- **Secrets Management**: Azure Key Vault via User-Assigned Managed Identity.
- **Observability**: Application Insights and Log Analytics Workspace.
- **AI Processing**: Azure AI Language Service (Text Analytics for Health).

## Deployment Guides

Detailed deployment instructions and strategies have been moved to dedicated documentation:

- [Azure Deployment Guide (Bicep & CLI)](azure-deployment-guide.md)
- [CI/CD Pipeline (GitHub Actions)](ci-cd.md)
- [Environment Strategy](environment-strategy.md)
- [Release Strategy](release-strategy.md)
- [Key Vault Strategy](key-vault-strategy.md)
- [Managed Identity Strategy](managed-identity-strategy.md)

*Bicep templates for these resources are located in `infra/bicep/`.*
