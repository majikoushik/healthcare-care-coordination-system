targetScope = 'subscription'

param location string = 'eastus'
param environmentName string = 'dev'
param appName string = 'healthco'

@secure()
param sqlAdminPassword string = newGuid() // Placeholder

resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rg-${appName}-${environmentName}-${location}'
  location: location
}

module logAnalytics 'modules/log-analytics.bicep' = {
  name: 'logAnalyticsDeploy'
  scope: rg
  params: {
    location: location
    workspaceName: 'law-${appName}-${environmentName}-${location}'
  }
}

module appInsights 'modules/application-insights.bicep' = {
  name: 'appInsightsDeploy'
  scope: rg
  params: {
    location: location
    appInsightsName: 'appi-${appName}-${environmentName}-${location}'
    logAnalyticsWorkspaceId: logAnalytics.outputs.workspaceId
  }
}

module keyVault 'modules/key-vault.bicep' = {
  name: 'keyVaultDeploy'
  scope: rg
  params: {
    location: location
    keyVaultName: 'kv-${appName}-${environmentName}-${location}'
  }
}

module managedIdentity 'modules/managed-identity.bicep' = {
  name: 'managedIdentityDeploy'
  scope: rg
  params: {
    location: location
    identityName: 'id-${appName}-${environmentName}-${location}'
  }
}

module acr 'modules/container-registry.bicep' = {
  name: 'acrDeploy'
  scope: rg
  params: {
    location: location
    acrName: 'cr${appName}${environmentName}${location}'
  }
}

module sqlDb 'modules/sql-database.bicep' = {
  name: 'sqlDbDeploy'
  scope: rg
  params: {
    location: location
    sqlServerName: 'sql-${appName}-${environmentName}-${location}'
    sqlDatabaseName: 'sqldb-${appName}-${environmentName}'
    adminUsername: 'sqladmin'
    adminPassword: sqlAdminPassword
  }
}

module cosmosDb 'modules/cosmos-db.bicep' = {
  name: 'cosmosDbDeploy'
  scope: rg
  params: {
    location: location
    accountName: 'cosmos-${appName}-${environmentName}-${location}'
    databaseName: 'HealthcareCareCoordination'
  }
}

module serviceBus 'modules/service-bus.bicep' = {
  name: 'serviceBusDeploy'
  scope: rg
  params: {
    location: location
    namespaceName: 'sb-${appName}-${environmentName}-${location}'
  }
}

module acaEnv 'modules/container-apps.bicep' = {
  name: 'acaEnvDeploy'
  scope: rg
  params: {
    location: location
    environmentName: 'cae-${appName}-${environmentName}-${location}'
    logAnalyticsWorkspaceId: logAnalytics.outputs.workspaceId
    containerApps: [
      {
        name: 'patient-api'
        image: 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest' // Placeholder
        externalIngress: true
        env: [
          { name: 'ASPNETCORE_ENVIRONMENT', value: 'Production' }
          { name: 'AI_PROVIDER', value: 'Mock' }
        ]
      }
      {
        name: 'provider-api'
        image: 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest' // Placeholder
        externalIngress: true
        env: [
          { name: 'ASPNETCORE_ENVIRONMENT', value: 'Production' }
          { name: 'AI_PROVIDER', value: 'Mock' }
        ]
      }
      {
        name: 'appointment-api'
        image: 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest' // Placeholder
        externalIngress: true
        env: [
          { name: 'ASPNETCORE_ENVIRONMENT', value: 'Production' }
          { name: 'AI_PROVIDER', value: 'Mock' }
        ]
      }
      {
        name: 'care-plan-api'
        image: 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest' // Placeholder
        externalIngress: true
        env: [
          { name: 'ASPNETCORE_ENVIRONMENT', value: 'Production' }
          { name: 'AI_PROVIDER', value: 'Mock' }
        ]
      }
      {
        name: 'clinical-insights-api'
        image: 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest' // Placeholder
        externalIngress: true
        env: [
          { name: 'ASPNETCORE_ENVIRONMENT', value: 'Production' }
          { name: 'AI_PROVIDER', value: 'Mock' }
        ]
      }
      {
        name: 'audit-api'
        image: 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest' // Placeholder
        externalIngress: true
        env: [
          { name: 'ASPNETCORE_ENVIRONMENT', value: 'Production' }
          { name: 'AI_PROVIDER', value: 'Mock' }
        ]
      }
    ]
  }
}

module staticWebApp 'modules/static-web-app.bicep' = {
  name: 'staticWebAppDeploy'
  scope: rg
  params: {
    location: location
    appName: 'stapp-${appName}-${environmentName}-${location}'
  }
}
