<#
.SYNOPSIS
    Creates a docs-only zip from the Khaos.RestApi solution.

.DESCRIPTION
    This script copies only Markdown files from the Khaos.RestApi solution
    into a temporary folder, then zips the result.

.PARAMETER Force
    If specified, overwrites existing zip and temp folder without prompting.

.EXAMPLE
    .\Clone-DocsOnly.ps1
    Creates a docs-only zip with confirmation prompt if output already exists.

.EXAMPLE
    .\Clone-DocsOnly.ps1 -Force
    Creates a docs-only zip, overwriting any existing output without prompting.
#>

param(
    [switch]$Force
)

$ErrorActionPreference = 'Stop'

# Get paths
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$solutionRoot = Split-Path -Parent $scriptDir
$parentDir = Split-Path -Parent $solutionRoot
$tempPath = Join-Path $parentDir "Khaos.RestApi.DocsOnly"
$zipPath = Join-Path $parentDir "Khaos.RestApi.DocsOnly.zip"

Write-Host "" 
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║           CREATE DOCS-ONLY RESTAPI ZIP                         ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

Write-Host "  Source:      $solutionRoot" -ForegroundColor Gray
Write-Host "  Temp Folder: $tempPath" -ForegroundColor Gray
Write-Host "  Zip Output:  $zipPath" -ForegroundColor Gray
Write-Host ""

function Confirm-Removal {
    param([string]$Path, [string]$Label)

    if (-not (Test-Path $Path)) {
        return $true
    }

    if ($Force) {
        Remove-Item -Path $Path -Recurse -Force
        return $true
    }

    $response = Read-Host "  $Label already exists. Remove it? (y/N)"
    if ($response -eq 'y' -or $response -eq 'Y') {
        Remove-Item -Path $Path -Recurse -Force
        return $true
    }

    Write-Host "  Aborted by user." -ForegroundColor Red
    return $false
}

# Step 1: Prepare outputs
Write-Host "────────────────────────────────────────────────────────────────" -ForegroundColor DarkGray
Write-Host "  Step 1: Preparing output..." -ForegroundColor Yellow
Write-Host "────────────────────────────────────────────────────────────────" -ForegroundColor DarkGray

if (-not (Confirm-Removal -Path $tempPath -Label "Temp folder")) { exit 1 }
if (-not (Confirm-Removal -Path $zipPath -Label "Zip file")) { exit 1 }

New-Item -ItemType Directory -Path $tempPath | Out-Null

# Step 2: Copy Markdown files
Write-Host "" 
Write-Host "────────────────────────────────────────────────────────────────" -ForegroundColor DarkGray
Write-Host "  Step 2: Collecting Markdown files..." -ForegroundColor Yellow
Write-Host "────────────────────────────────────────────────────────────────" -ForegroundColor DarkGray

$mdFiles = Get-ChildItem -Path $solutionRoot -Recurse -File -Filter "*.md" -ErrorAction SilentlyContinue

foreach ($file in $mdFiles) {
    $relative = $file.FullName.Substring($solutionRoot.Length).TrimStart('\','/')
    $destination = Join-Path $tempPath $relative
    $destinationDir = Split-Path -Parent $destination
    if (-not (Test-Path $destinationDir)) {
        New-Item -ItemType Directory -Path $destinationDir -Force | Out-Null
    }
    Copy-Item -Path $file.FullName -Destination $destination -Force
}

Write-Host "  ✓ Copied $($mdFiles.Count) Markdown files." -ForegroundColor Green

# Step 3: Zip
Write-Host "" 
Write-Host "────────────────────────────────────────────────────────────────" -ForegroundColor DarkGray
Write-Host "  Step 3: Creating zip..." -ForegroundColor Yellow
Write-Host "────────────────────────────────────────────────────────────────" -ForegroundColor DarkGray

Compress-Archive -Path (Join-Path $tempPath "*") -DestinationPath $zipPath -Force

# Step 4: Cleanup
Write-Host "" 
Write-Host "────────────────────────────────────────────────────────────────" -ForegroundColor DarkGray
Write-Host "  Step 4: Cleaning up..." -ForegroundColor Yellow
Write-Host "────────────────────────────────────────────────────────────────" -ForegroundColor DarkGray

Remove-Item -Path $tempPath -Recurse -Force

Write-Host "" 
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║                      ZIP COMPLETE                              ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""
Write-Host "  Zip Location: $zipPath" -ForegroundColor White
Write-Host "  Files:        $($mdFiles.Count)" -ForegroundColor White
Write-Host "" 
