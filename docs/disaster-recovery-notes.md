# Disaster Recovery Notes

While this is a demo environment, enterprise healthcare platforms require strict disaster recovery (DR) protocols.

## RPO and RTO Targets
- **Recovery Point Objective (RPO)**: Target < 1 hour for transactional data.
- **Recovery Time Objective (RTO)**: Target < 4 hours for full system restoration.

## Data Redundancy
- **Azure SQL**: Geo-redundant backups are enabled by default, allowing cross-region restore in the event of an Azure region failure.
- **Cosmos DB**: The blueprint provisions Cosmos DB in a single region for cost savings, but global distribution (multi-region writes) can be toggled via Bicep for production-grade DR.

## Compute Restoration
- All compute (Container Apps, Static Web Apps) is stateless. Restoration simply requires re-running the GitHub Actions deployment pipeline against a secondary Azure region. Traffic Manager or Azure Front Door would handle DNS failover in a true production setup.
