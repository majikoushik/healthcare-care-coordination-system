# Azure Deployment Guide

This guide details the process for deploying the `healthcare-care-coordination-system` to Microsoft Azure using Bicep infrastructure-as-code and GitHub Actions.

## Prerequisites
1. **Azure CLI**: Installed and authenticated (`az login`).
2. **Azure Subscription**: Target subscription for deployment.
3. **GitHub Secrets**: Ensure required secrets are populated in the GitHub repository.

## Azure Resource Overview
The deployment provisions:
- Azure Static Web Apps (React Frontend)
- Azure Container Apps Environment & Apps (.NET Backend)
- Azure Container Registry (Images)
- Azure SQL Database (Transactional Data)
- Azure Cosmos DB (Documents & Events)
- Azure Key Vault (Secrets Management)
- Application Insights & Log Analytics (Telemetry)

## Required GitHub Secrets
To enable the CI/CD pipeline, configure the following GitHub Secrets:
- `AZURE_CLIENT_ID`
- `AZURE_TENANT_ID`
- `AZURE_SUBSCRIPTION_ID`
- `DEV_AZURE_RESOURCE_GROUP`
- `DEV_AZURE_CONTAINER_REGISTRY`

*Note: We use OpenID Connect (OIDC) authentication (`AZURE_CLIENT_ID`) instead of legacy service principal passwords (`AZURE_CREDENTIALS`).*

## Local Provisioning via Bicep (Placeholder Example)
You can test the Bicep templates locally using placeholder commands:

```powershell
az group create --name rg-healthco-dev-eastus --location eastus
az deployment group create --resource-group rg-healthco-dev-eastus --template-file infra/bicep/main.bicep --parameters infra/bicep/parameters/dev.parameters.json
```

## Security & Healthcare Privacy Warning
- **Synthetic Data**: The deployed environment must only be used with synthetic data. No real patient data should be loaded into this demo architecture.
- **Secrets Management**: No secrets are hardcoded in the codebase or Bicep files. Passwords (like `sqlAdminPassword`) should be passed dynamically via Key Vault references or GitHub action secret injection in production scenarios.
