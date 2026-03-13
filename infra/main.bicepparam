// Kairudev - Paramètres Bicep (environnement de développement)
// Ce fichier contient les valeurs NON sensibles. Les secrets sont passés via CLI ou GitHub Secrets.

using './main.bicep'

param environment = 'dev'
param location = 'francecentral'
param appName = 'kairudev'
param sqlAdminLogin = 'kairudevadmin'

// Secrets à passer via CLI :
// --parameters githubClientId='...' githubClientSecret='...' jwtSecretKey='...' sqlAdminPassword='...'
