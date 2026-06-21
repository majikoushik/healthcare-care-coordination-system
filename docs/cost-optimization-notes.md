# Cost Optimization Notes

As a portfolio demo, minimizing Azure spend while demonstrating enterprise patterns is critical.

## Current Cost-Saving Measures
- **Azure Static Web Apps**: Using the `Free` tier.
- **Azure Container Apps**: Configured to scale down to 0 replicas (`minReplicas: 0` in non-prod environments) to avoid idle compute charges.
- **Azure SQL Database**: Utilizing the `Basic` tier (5 DTUs).
- **Cosmos DB**: Using a provisioned throughput minimum (400 RU/s) or exploring Serverless capacity modes.
- **Log Analytics**: Data retention limited to 30 days.

## Future Considerations
- Utilize Azure Advisor to identify over-provisioned Container Apps.
- Set up Azure Budgets and Action Groups to alert if spend exceeds $10/month during portfolio demonstration windows.
