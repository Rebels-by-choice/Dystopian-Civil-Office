$ErrorActionPreference = "Stop"
$Host.UI.RawUI.WindowTitle = "DCO production compose"

Write-Host "=== Dystopian Civil Office: starting production profile ===" -ForegroundColor Cyan

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = (Resolve-Path (Join-Path $scriptDir "..\..\..")).Path

function Assert-CommandExists {
    param([string]$CommandName)

    if (-not (Get-Command $CommandName -ErrorAction SilentlyContinue)) {
        throw "Required command '$CommandName' was not found in PATH."
    }
}

Assert-CommandExists "docker"

Set-Location $repoRoot

Write-Host ""
Write-Host "==> Running Docker Compose" -ForegroundColor Yellow
docker compose --profile production up -d --build

Write-Host ""
Write-Host "Environment started successfully." -ForegroundColor Green
Write-Host "Project root: $repoRoot" -ForegroundColor DarkGray
Write-Host ""
Write-Host "Useful commands:" -ForegroundColor Cyan
Write-Host "  docker compose ps"
Write-Host "  docker compose logs -f"
Write-Host "  docker compose --profile production down"
Write-Host ""