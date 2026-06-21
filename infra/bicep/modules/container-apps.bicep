param location string = resourceGroup().location
param environmentName string
param logAnalyticsWorkspaceId string
param containerApps array

resource acaEnvironment 'Microsoft.App/managedEnvironments@2023-05-01' = {
  name: environmentName
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: reference(logAnalyticsWorkspaceId, '2022-10-01').customerId
        sharedKey: listKeys(logAnalyticsWorkspaceId, '2022-10-01').primarySharedKey
      }
    }
  }
}

resource apps 'Microsoft.App/containerApps@2023-05-01' = [for app in containerApps: {
  name: app.name
  location: location
  properties: {
    managedEnvironmentId: acaEnvironment.id
    configuration: {
      ingress: {
        external: app.externalIngress
        targetPort: 8080
      }
    }
    template: {
      containers: [
        {
          name: app.name
          image: app.image
          resources: {
            cpu: json('0.5')
            memory: '1.0Gi'
          }
          env: app.env
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 5
      }
    }
  }
}]

output environmentId string = acaEnvironment.id
