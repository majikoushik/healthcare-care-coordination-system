# Release Strategy

We employ a trunk-based development flow adapted for multi-environment Azure delivery.

## Workflow
1. **Feature Branches**: Developers work on `feature/*` branches locally using Mock data.
2. **Pull Requests**: PRs against `develop` trigger the CI workflow (Build, Test, Validate Dockerfiles). No deployment occurs.
3. **Merge to Develop**: Triggers the `azure-deploy-dev.yml` workflow. Bicep and images are deployed to the `Dev` Azure environment.
4. **Release Promotion**: When `develop` is stable, a `release/vX.X` branch is cut. This triggers deployment to `Test`.
5. **Production Release**: Merging the release branch into `main` and tagging triggers the production pipeline (which requires manual approval gates in GitHub Environments).

## Zero Downtime
- Frontend: Static Web Apps serve new assets globally.
- Backend: Container App Revisions transition traffic smoothly. Database schema migrations must always be backwards compatible.
