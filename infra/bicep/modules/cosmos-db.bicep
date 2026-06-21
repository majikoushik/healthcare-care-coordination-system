param location string = resourceGroup().location
param accountName string
param databaseName string

resource cosmosDbAccount 'Microsoft.DocumentDB/databaseAccounts@2023-04-15' = {
  name: accountName
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
        failoverPriority: 0
      }
    ]
  }
}

resource sqlDatabase 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2023-04-15' = {
  parent: cosmosDbAccount
  name: databaseName
  properties: {
    resource: {
      id: databaseName
    }
  }
}

var containers = [
  { name: 'care-plans', partitionKey: '/patientId' }
  { name: 'clinical-insights', partitionKey: '/patientId' }
  { name: 'follow-up-tasks', partitionKey: '/patientId' }
  { name: 'notifications', partitionKey: '/patientId' }
  { name: 'audit-events', partitionKey: '/correlationId' }
]

resource sqlContainers 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = [for container in containers: {
  parent: sqlDatabase
  name: container.name
  properties: {
    resource: {
      id: container.name
      partitionKey: {
        paths: [
          container.partitionKey
        ]
        kind: 'Hash'
      }
    }
  }
}]

output cosmosDbEndpoint string = cosmosDbAccount.properties.documentEndpoint
output cosmosDbId string = cosmosDbAccount.id
