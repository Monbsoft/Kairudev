// Kairudev - Paramètres Bicep (environnement de production)
// Ce fichier contient les valeurs NON sensibles. Les secrets sont passés via CLI ou GitHub Secrets.

using './main.bicep'

param environment = 'prod'
param location = 'francecentral'
param appName = 'kairudev'
param sqlAdminLogin = 'kairudevadmin'

// Secrets à passer via CLI :
// --parameters githubClientId='...' githubClientSecret='...' jwtSecretKey='...' sqlAdminPassword='...'
