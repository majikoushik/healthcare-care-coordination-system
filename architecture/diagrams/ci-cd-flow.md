# CI/CD Flow

```mermaid
flowchart TD
    Developer[Developer] -->|Push/PR| GitHub[GitHub Repo]
    
    subgraph "GitHub Actions Workflow"
        GitHub --> Trigger{Branch `main` or `develop`?}
        Trigger -->|Yes| Job1(Backend Build & Test)
        Trigger -->|Yes| Job2(Frontend Build & Test)
        
        Job1 --> DotNetRestore[dotnet restore]
        DotNetRestore --> DotNetBuild[dotnet build]
        DotNetBuild --> DotNetTest[dotnet test (xUnit)]
        
        Job2 --> NpmInstall[npm install]
        NpmInstall --> NpmTsc[npx tsc --noEmit]
        NpmTsc --> NpmBuild[npm run build]
        NpmBuild --> NpmTest[npm test]
        
        DotNetTest --> Job3(Docker Build Validation)
        NpmTest --> Job3
        
        Job3 --> DockerBuildBackend[Build Patient API Dockerfile]
        Job3 --> DockerBuildFrontend[Build Portal Dockerfile]
        
        DockerBuildBackend --> Status{Pass/Fail}
        DockerBuildFrontend --> Status
    end
    
    Status -->|Pass| Ready[PR Ready to Merge]
```
