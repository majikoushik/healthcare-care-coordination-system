# Secure Healthcare Data Handling

This project demonstrates secure healthcare data handling patterns. However, **all data used in this project must be synthetic**.

## Privacy by Design Principles

1. **Synthetic Data Only**: This system does not and will not process real patient medical records.
2. **Safe Logging**: We avoid logging full patient profiles. Logs only contain non-sensitive metadata, such as:
   - Patient ID (GUID)
   - Correlation ID
   - Operation Type (e.g., "Patient updated")
3. **No Sensitive Health Data in Master Profile**: The `Patient` entity only contains demographic and contact data, along with a high-level `ConsentStatus`. Clinical specifics belong in Cosmos DB documents securely, without exposing them to unauthorized boundaries.
4. **Error Handling**: Exceptions and validation failures do not leak backend stack traces or database configurations. We use standard Problem Details format.

## Future RBAC Readiness
Authentication and role-based access control (RBAC) are planned for future epics to simulate secure tenant and provider access boundaries using standard JWT approaches.
