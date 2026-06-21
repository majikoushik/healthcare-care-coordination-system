# Monitoring and Alerting Strategy

This repository is designed for Enterprise Observability via **Azure Monitor** and **Application Insights**.

## Alerting Strategy
The system handles expected domain errors (e.g. Invalid Appointment Time) via `400 Bad Request` Problem Details. These should NOT trigger pager alerts. Alerts are reserved for Systemic failures.

### Recommended Alert Rules

1. **API Error Rate Increase**
   - **Condition**: Count of `HTTP 5xx` responses > 5% over 5 minutes.
   - **Action**: Page Care Coordination DevOps Team.

2. **API Latency Increase**
   - **Condition**: P95 Latency of `/api/v1/clinical-insights/analyze` > 5 seconds over 5 minutes.
   - **Action**: Alert AI Ops Team. Indicates Azure Language API throttling.

3. **SQL Dependency Failure**
   - **Condition**: Failed calls to `PatientDb` > 10 in 5 minutes.
   - **Action**: Page Database Team.

4. **Cosmos DB Dependency Failure**
   - **Condition**: Request Charge (RU/s) exceeded `429 Too Many Requests` > 50 in 1 minute.
   - **Action**: Scale Cosmos DB partition or page DevOps.

5. **Notification Delivery Failure Spike**
   - **Condition**: Simulated (or actual) Send failures > 20%.
   - **Action**: Alert Ops Team.

6. **Health Endpoint Failure**
   - **Condition**: `/health/ready` returns `Unhealthy` for 3 consecutive polls (every 30s).
   - **Action**: Remove container from load balancer; Page On-call.

7. **Unauthorized / Forbidden Spike**
   - **Condition**: `HTTP 401/403` count > 100 in 5 minutes.
   - **Action**: Alert Security Team (potential brute force or misconfigured RBAC token).
