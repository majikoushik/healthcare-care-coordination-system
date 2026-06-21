# CI/CD Pipeline

The Continuous Integration (CI) and Continuous Deployment (CD) pipeline for `healthcare-care-coordination-system` uses **GitHub Actions**.

## Workflow File

The main workflow is defined at `.github/workflows/ci.yml`.

## Triggers
- `push` to `main` or `develop` branches.
- `pull_request` against `main` or `develop`.

## Jobs

The CI workflow is divided into three parallel/dependent jobs.

### 1. Backend Job (`backend`)
- **Environment**: Ubuntu Latest
- **Setup**: Configures .NET 8 SDK.
- **Steps**:
  - Restores dependencies using `dotnet restore`.
  - Builds all solutions and `.csproj` files (`dotnet build --configuration Release --no-restore`).
  - Runs all xUnit tests (`dotnet test`).
- **Secrets/Env**: Uses mock variables (`AI_PROVIDER=Mock`, `COSMOS_MODE=Mock`) so it does not require real Azure resources.

### 2. Frontend Job (`frontend`)
- **Environment**: Ubuntu Latest
- **Setup**: Node.js 22.
- **Steps**:
  - `npm ci` for lockfile-based dependencies.
  - Type-checking with `npx tsc --noEmit`.
  - Production build generation (`npm run build`).
  - Runs React testing library tests.

### 3. Docker Validation Job (`docker-validation`)
- **Dependencies**: Depends on both `backend` and `frontend` jobs passing.
- **Steps**:
  - Test-builds the `Patient.Api` Docker image to ensure the multi-stage `.NET` Dockerfile is valid.
  - Test-builds the `healthcare-care-portal` Nginx image.
  - *Note: These images are not pushed to a registry during PRs, but validate that containerization remains un-broken.*

## Continuous Deployment (Azure Readiness)
Azure deployment workflows are template-based and require GitHub OIDC secrets. The Bicep template is subscription-scoped because it creates the target resource group.
