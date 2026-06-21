# Security Architecture

Epic 0 prepares security posture without implementing authentication.

- Future identity: Azure Entra ID or Entra External ID.
- Future authorization: JWT validation and role-based access control.
- Prepared roles: Patient, Provider, CareCoordinator, Admin, Auditor.
- Configuration: safe placeholders only; secrets belong in user secrets, environment variables, or Azure Key Vault.
- API safety: Problem Details errors avoid leaking sensitive data.
