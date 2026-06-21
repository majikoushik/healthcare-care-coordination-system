# Azure Network Flow

```mermaid
graph TD
    Internet((Internet)) -->|HTTPS| ASWA(Azure Static Web Apps)
    Internet -.->|HTTPS (Optional APIM)| APIM(API Management)
    
    ASWA --> ACA_Env
    APIM --> ACA_Env
    
    subgraph "VNet (Future Hardening)"
        ACA_Env(Azure Container Apps Env)
        SQL(Azure SQL Database / Private Endpoint)
        Cosmos(Azure Cosmos DB / Private Endpoint)
        KV(Key Vault / Private Endpoint)
        
        ACA_Env --> SQL
        ACA_Env --> Cosmos
        ACA_Env --> KV
    end
```
