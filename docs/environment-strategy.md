# Environment Strategy

The `healthcare-care-coordination-system` utilizes a three-tier environment architecture to ensure code quality, isolated testing, and stable production releases.

## 1. Development (Dev)
- **Branch**: `develop`
- **Purpose**: Continuous Integration target. All developer pull requests merge here.
- **Data**: 100% Mock/Synthetic. Databases are routinely purged and seeded.
- **Scale**: Minimum instance counts (Min: 0, Max: 2) in Container Apps. Basic tier SQL/Cosmos.
- **Naming Convention**: `*-healthco-dev-*`

## 2. Test/Staging (Test)
- **Branch**: `release/*`
- **Purpose**: Pre-production validation, integration testing, and user acceptance.
- **Data**: Synthetic but production-like volume.
- **Scale**: Moderate scaling rules reflecting realistic load.
- **Naming Convention**: `*-healthco-test-*`

## 3. Production (Prod)
- **Branch**: `main` (via tagged releases)
- **Purpose**: Live environment (Demo portfolio representation).
- **Data**: Isolated. Strict RBAC.
- **Scale**: Highly available (Min: 2, Max: 10), Geo-redundant SQL/Cosmos backups.
- **Naming Convention**: `*-healthco-prod-*`

## Naming Conventions
We follow Azure recommended naming standards:
- `<ResourceType>-<AppName>-<Environment>-<Region>`
- Example: `rg-healthco-dev-eastus`, `kv-healthco-prod-eastus`
