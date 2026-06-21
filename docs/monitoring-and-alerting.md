# Monitoring and Alerting

This document outlines the Azure-native observability strategy for the care coordination platform.

## Application Insights Integration
All backend services use the Application Insights SDK. Logs emitted by Serilog are forwarded directly into App Insights. 
- **Correlation ID**: The `X-Correlation-ID` header is tracked natively as an Operation ID in App Insights, enabling end-to-end tracing from the React frontend to the database layer.

## Log Analytics Workspace
App Insights and Azure Container Apps Environment logs flow into a central Log Analytics Workspace.
- Custom KQL (Kusto Query Language) can be written to extract business metrics (e.g., number of Care Plans created today).

## Recommended Alert Rules

1. **API Error Rate Spike**: Alert if HTTP 5xx responses exceed 5% over a 5-minute window.
2. **API Latency Increase**: Alert if 95th percentile response time exceeds 2000ms.
3. **Container App Restart Count**: Alert if any container restarts more than 3 times in 1 hour (indicates crash loops or memory leaks).
4. **SQL Database Connectivity Failure**: Alert on specific SQL Exceptions in App Insights traces.
5. **Cosmos DB RU Pressure**: Alert if consumed Request Units (RUs) hit 90% of provisioned capacity.
6. **Clinical AI Provider Failure**: Alert if the Azure AI Language service returns non-200 status codes.
7. **Unauthorized Request Spike**: Alert if HTTP 401/403 errors exceed normal baselines (potential brute force/reconnaissance).
