$ErrorActionPreference = "Stop"
$Host.UI.RawUI.WindowTitle = "Main init & frontend Angular logs"

Write-Host "=== Dystopian Civil Office: starting development environment ===" -ForegroundColor Cyan

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Resolve-Path (Join-Path $scriptDir "..\..\..")

$serverPath = Join-Path $repoRoot "apps\server\src\Dystopian-Civil-Office\Dystopian-Civil-Office"
$clientPath = Join-Path $repoRoot "apps\client\Dystopian-Civil-Office"

function Start-Step {
    param([string]$Message)
    Write-Host ""
    Write-Host "==> $Message" -ForegroundColor Yellow
}

function Assert-CommandExists {
    param([string]$CommandName)

    if (-not (Get-Command $CommandName -ErrorAction SilentlyContinue)) {
        throw "Required command '$CommandName' was not found in PATH."
    }
}

function Open-PowerShellWindow {
    param(
        [string]$Title,
        [string]$WorkingDirectory,
        [string]$Command
    )

    $psCommand = @"
`$Host.UI.RawUI.WindowTitle = '$Title'
Set-Location '$WorkingDirectory'
$Command
"@

    Start-Process `
        -FilePath "powershell.exe" `
        -ArgumentList "-NoExit", "-ExecutionPolicy", "Bypass", "-Command", $psCommand `
        -WorkingDirectory $WorkingDirectory `
        -PassThru
}

Assert-CommandExists "docker"
Assert-CommandExists "dotnet"
Assert-CommandExists "npm"
Assert-CommandExists "npx"

Start-Step "Starting Docker containers"
Push-Location $repoRoot
docker compose up -d --build
Pop-Location

Start-Sleep -Seconds 3
Write-Host "Docker containers started successfully." -ForegroundColor Green

Start-Step "Running EF Core migrations"
Push-Location $serverPath
dotnet ef database update
Pop-Location
Write-Host "Database migration completed." -ForegroundColor Green

Start-Step "Opening containers logs window"
$containersProcess = Open-PowerShellWindow `
    -Title "Containers logs" `
    -WorkingDirectory $repoRoot `
    -Command "docker compose logs -f"

Write-Host "Containers logs window started. PID: $($containersProcess.Id)" -ForegroundColor Green

Start-Step "Opening backend logs window"
$backendProcess = Open-PowerShellWindow `
    -Title "Backend .NET logs" `
    -WorkingDirectory $serverPath `
    -Command "dotnet watch run"

Write-Host "Backend logs window started. PID: $($backendProcess.Id)" -ForegroundColor Green

Start-Step "Installing frontend dependencies"
Push-Location $clientPath
npm ci
Pop-Location
Write-Host "Frontend dependencies installed." -ForegroundColor Green

Start-Step "Starting frontend in current window"
Set-Location $clientPath
$env:NG_CLI_ANALYTICS = "false"

Write-Host ""
Write-Host "Development environment is running." -ForegroundColor Cyan
Write-Host "Containers logs PID : $($containersProcess.Id)"
Write-Host "Backend logs PID    : $($backendProcess.Id)"
Write-Host "Frontend logs       : current window"
Write-Host ""
Write-Host "Press Ctrl + C in a given window to stop its process." -ForegroundColor DarkYellow
Write-Host ""

npx ng serve