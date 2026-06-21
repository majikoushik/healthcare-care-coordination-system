# Rollback Strategy

## Application Rollbacks (Azure Container Apps)
Azure Container Apps supports revision management natively, making rollbacks instantaneous.
- **Zero-Downtime Deployments**: When a new image is deployed, a new Revision is created. Traffic is shifted to the new revision only when it passes health probes.
- **Rollback Process**: If a deployment fails or exhibits high error rates, traffic can be instantly reverted to the previous known-good Revision via the Azure Portal or CLI.

## Infrastructure Rollbacks (Bicep)
- Infrastructure deployments are declarative and idempotent. Rolling back infrastructure involves reverting the Bicep template code in `main` and running the deployment pipeline again to enforce the previous state.

## Database Rollbacks
- Azure SQL and Cosmos DB are backed up automatically.
- Point-in-Time Restore (PITR) is the primary fallback for catastrophic data corruption.
- Destructive schema changes (e.g., dropping columns) are strictly prohibited in forward-moving migrations.
