param location string = resourceGroup().location
param acrName string
param sku string = 'Basic'

resource acr 'Microsoft.ContainerRegistry/registries@2023-01-01-preview' = {
  name: acrName
  location: location
  sku: {
    name: sku
  }
  properties: {
    adminUserEnabled: false // Best practice: Use Managed Identity
  }
}

output acrId string = acr.id
output acrLoginServer string = acr.properties.loginServer
