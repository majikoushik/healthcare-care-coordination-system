# Key Vault Strategy

Azure Key Vault acts as the central, secure repository for all application secrets, connection strings, and certificates.

## What is Stored in Key Vault?
- **SQL Connection Strings**: Access to Azure SQL Database.
- **Cosmos DB Endpoints & Keys**: Although Managed Identity is preferred for Cosmos DB, Keys are stored here for legacy/fallback support.
- **Azure AI Language Keys**: If the application requires the real AI endpoint, the Key is stored here.
- **Application Insights Connection Strings**: Stored to decouple telemetry from hardcoded values.

## Access Patterns
1. **No direct credential passing**: Container Apps do not hold raw secrets. They define environment variables referencing Key Vault URIs using the `secretref` syntax.
2. **Managed Identity Authorization**: Container Apps authenticate to Key Vault via User-Assigned Managed Identity. No client secrets are necessary in the application code.
3. **Local Development**: Local development bypasses Key Vault completely, relying on safe `.env` mock placeholders, avoiding developer credential leakage.
