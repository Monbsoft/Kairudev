#!/usr/bin/env pwsh
# Kairudev - Script de déploiement Azure
# Usage: .\deploy.ps1 -Environment dev -ResourceGroupName rg-kairudev-dev

[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [ValidateSet('dev', 'staging', 'prod')]
    [string]$Environment,

    [Parameter(Mandatory=$true)]
    [string]$ResourceGroupName,

    [Parameter(Mandatory=$false)]
    [string]$Location = 'francecentral',

    [Parameter(Mandatory=$false)]
    [string]$SubscriptionId,

    [Parameter(Mandatory=$false)]
    [switch]$WhatIf
)

$ErrorActionPreference = 'Stop'

Write-Host "🚀 Déploiement Kairudev sur Azure - Environnement: $Environment" -ForegroundColor Cyan

# Connexion Azure
Write-Host "`n📝 Vérification de la connexion Azure..." -ForegroundColor Yellow
try {
    $context = Get-AzContext
    if ($null -eq $context) {
        throw "Pas de contexte Azure actif"
    }
    Write-Host "✅ Connecté en tant que: $($context.Account.Id)" -ForegroundColor Green

    if ($SubscriptionId) {
        Write-Host "🔄 Changement de subscription vers: $SubscriptionId" -ForegroundColor Yellow
        Set-AzContext -SubscriptionId $SubscriptionId | Out-Null
    }

    $context = Get-AzContext
    Write-Host "✅ Subscription active: $($context.Subscription.Name)" -ForegroundColor Green
} catch {
    Write-Host "❌ Erreur de connexion Azure. Exécutez 'Connect-AzAccount' d'abord." -ForegroundColor Red
    exit 1
}

# Création du Resource Group si nécessaire
Write-Host "`n📦 Vérification du Resource Group: $ResourceGroupName" -ForegroundColor Yellow
$rg = Get-AzResourceGroup -Name $ResourceGroupName -ErrorAction SilentlyContinue
if ($null -eq $rg) {
    Write-Host "🆕 Création du Resource Group: $ResourceGroupName dans $Location" -ForegroundColor Yellow
    if (-not $WhatIf) {
        New-AzResourceGroup -Name $ResourceGroupName -Location $Location | Out-Null
        Write-Host "✅ Resource Group créé" -ForegroundColor Green
    } else {
        Write-Host "⚠️  [WhatIf] Resource Group serait créé" -ForegroundColor Yellow
    }
} else {
    Write-Host "✅ Resource Group existe déjà" -ForegroundColor Green
}

# Récupération des secrets
Write-Host "`n🔐 Récupération des secrets..." -ForegroundColor Yellow

# GitHub OAuth
$githubClientId = $env:GITHUB_CLIENT_ID
$githubClientSecret = $env:GITHUB_CLIENT_SECRET

if ([string]::IsNullOrEmpty($githubClientId) -or [string]::IsNullOrEmpty($githubClientSecret)) {
    Write-Host "❌ Variables d'environnement GITHUB_CLIENT_ID et GITHUB_CLIENT_SECRET requises" -ForegroundColor Red
    Write-Host "💡 Définissez-les avec: `$env:GITHUB_CLIENT_ID='...'; `$env:GITHUB_CLIENT_SECRET='...'" -ForegroundColor Cyan
    exit 1
}

# JWT Secret
$jwtSecretKey = $env:JWT_SECRET_KEY
if ([string]::IsNullOrEmpty($jwtSecretKey)) {
    Write-Host "⚠️  JWT_SECRET_KEY non défini, génération automatique..." -ForegroundColor Yellow
    $jwtSecretKey = [Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32))
    Write-Host "✅ JWT Secret généré (sauvegardez-le pour les futurs déploiements)" -ForegroundColor Green
    Write-Host "💡 JWT_SECRET_KEY=$jwtSecretKey" -ForegroundColor Cyan
}

# SQL Admin Password
$sqlAdminPassword = $env:SQL_ADMIN_PASSWORD
if ([string]::IsNullOrEmpty($sqlAdminPassword)) {
    Write-Host "❌ Variable d'environnement SQL_ADMIN_PASSWORD requise" -ForegroundColor Red
    Write-Host "💡 Définissez-la avec: `$env:SQL_ADMIN_PASSWORD='YourStrongPassword123!'" -ForegroundColor Cyan
    exit 1
}

# Sélection du fichier de paramètres
$bicepParamFile = if ($Environment -eq 'prod') { 'main.prod.bicepparam' } else { 'main.bicepparam' }
$bicepParamPath = Join-Path $PSScriptRoot $bicepParamFile

if (-not (Test-Path $bicepParamPath)) {
    Write-Host "❌ Fichier de paramètres introuvable: $bicepParamPath" -ForegroundColor Red
    exit 1
}

# Déploiement
Write-Host "`n🚀 Déploiement de l'infrastructure Bicep..." -ForegroundColor Yellow
Write-Host "📄 Fichier de paramètres: $bicepParamFile" -ForegroundColor Cyan
Write-Host "📦 Resource Group: $ResourceGroupName" -ForegroundColor Cyan
Write-Host "🌍 Location: $Location" -ForegroundColor Cyan

$deploymentName = "kairudev-$Environment-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

$deployParams = @{
    Name = $deploymentName
    ResourceGroupName = $ResourceGroupName
    TemplateParameterFile = $bicepParamPath
    githubClientId = $githubClientId
    githubClientSecret = ConvertTo-SecureString -String $githubClientSecret -AsPlainText -Force
    jwtSecretKey = ConvertTo-SecureString -String $jwtSecretKey -AsPlainText -Force
    sqlAdminPassword = ConvertTo-SecureString -String $sqlAdminPassword -AsPlainText -Force
}

if ($WhatIf) {
    $deployParams.Add('WhatIf', $true)
    Write-Host "⚠️  Mode WhatIf activé - aucun changement ne sera appliqué" -ForegroundColor Yellow
}

try {
    $deployment = New-AzResourceGroupDeployment @deployParams

    if ($WhatIf) {
        Write-Host "`n✅ Validation WhatIf réussie" -ForegroundColor Green
        exit 0
    }

    if ($deployment.ProvisioningState -eq 'Succeeded') {
        Write-Host "`n✅ Déploiement réussi!" -ForegroundColor Green
        Write-Host "`n📊 Outputs:" -ForegroundColor Cyan
        $deployment.Outputs.Keys | ForEach-Object {
            Write-Host "  $_ : $($deployment.Outputs[$_].Value)" -ForegroundColor White
        }

        Write-Host "`n🌐 URL de l'application: $($deployment.Outputs['webAppUrl'].Value)" -ForegroundColor Green
        Write-Host "`n📝 Prochaines étapes:" -ForegroundColor Yellow
        Write-Host "  1. Configurer les migrations EF Core (voir docs/deploy.md)" -ForegroundColor White
        Write-Host "  2. Déployer le code de l'API (voir .github/workflows/azure-deploy.yml)" -ForegroundColor White
        Write-Host "  3. Configurer le callback GitHub OAuth: $($deployment.Outputs['webAppUrl'].Value)/api/auth/github/callback" -ForegroundColor White
    } else {
        Write-Host "`n❌ Déploiement échoué: $($deployment.ProvisioningState)" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "`n❌ Erreur lors du déploiement: $_" -ForegroundColor Red
    Write-Host $_.ScriptStackTrace -ForegroundColor Red
    exit 1
}
