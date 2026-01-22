param(
    [Parameter(Mandatory = $true)][string]$ApiName,
    [string]$DbSchema = "Audit",
    [ValidateSet("Single", "Split")][string]$TableMode = "Single",
    [bool]$EnableAuditing = $true,
    [string]$SolutionFolder = "src"
)

$repoRoot = Resolve-Path "$PSScriptRoot/.."
$templatePath = Join-Path $repoRoot "templates/dotnet-new-external"

Write-Host "Installing local template pack from $templatePath" -ForegroundColor Cyan
& dotnet new install $templatePath | Out-Null

Write-Host "Scaffolding API module '$ApiName'..." -ForegroundColor Green
& dotnet new khaos-apimodule `
    --ApiName $ApiName `
    --DbSchema $DbSchema `
    --TableMode $TableMode `
    --EnableAuditing $EnableAuditing `
    --SolutionFolder $SolutionFolder `
    --output $repoRoot
