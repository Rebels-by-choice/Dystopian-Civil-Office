$ErrorActionPreference = "Stop"
$Host.UI.RawUI.WindowTitle = "Main init & frontend Angular logs"

Write-Host "=== Dystopian Civil Office: starting development environment ===" -ForegroundColor Cyan

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = (Resolve-Path (Join-Path $scriptDir "..\..\..")).Path

$serverPath = Join-Path $repoRoot "apps\server\src\Dystopian-Civil-Office\Dystopian-Civil-Office"
$clientPath = Join-Path $repoRoot "apps\client\Dystopian-Civil-Office"
$logsPath = Join-Path $repoRoot "logs"

$timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
$containersLogPath = Join-Path $logsPath "containers-$timestamp.log"
$backendLogPath = Join-Path $logsPath "backend-$timestamp.log"
$frontendLogPath = Join-Path $logsPath "frontend-$timestamp.log"

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

function Remove-OldLogs {
    param(
        [string]$Path,
        [int]$Days = 7
    )

    if (-not (Test-Path $Path)) {
        return
    }

    $cutoffDate = (Get-Date).AddDays(-$Days)

    Get-ChildItem -Path $Path -File -ErrorAction SilentlyContinue |
        Where-Object { $_.LastWriteTime -lt $cutoffDate } |
        Remove-Item -Force -ErrorAction SilentlyContinue
}

function Open-PowerShellWindow {
    param(
        [string]$Title,
        [string]$WorkingDirectory,
        [string]$Command,
        [string]$LogFilePath
    )

    $escapedWorkingDirectory = $WorkingDirectory.Replace("'", "''")
    $escapedLogFilePath = $LogFilePath.Replace("'", "''")

    $psCommand = @"
`$Host.UI.RawUI.WindowTitle = '$Title'
Set-Location '$escapedWorkingDirectory'
& {
    $Command
} 2>&1 | Tee-Object -FilePath '$escapedLogFilePath' -Append
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

New-Item -ItemType Directory -Path $logsPath -Force | Out-Null
Remove-OldLogs -Path $logsPath -Days 7

Start-Step "Starting Docker containers"
Push-Location $repoRoot
docker compose up -d --build
Pop-Location

Start-Sleep -Seconds 3
Write-Host "Docker containers started successfully." -ForegroundColor Green

Start-Step "Restoring backend packages"
Push-Location $serverPath
dotnet restore
Pop-Location
Write-Host "Backend packages restored." -ForegroundColor Green

Start-Step "Running EF Core migrations"
Push-Location $serverPath
dotnet ef database update
Pop-Location
Write-Host "Database migration completed." -ForegroundColor Green

Start-Step "Opening containers logs window"
$containersProcess = Open-PowerShellWindow `
    -Title "Containers logs" `
    -WorkingDirectory $repoRoot `
    -Command "docker compose logs -f" `
    -LogFilePath $containersLogPath

Write-Host "Containers logs window started. PID: $($containersProcess.Id)" -ForegroundColor Green
Write-Host "Containers log file: $containersLogPath" -ForegroundColor DarkGray

Start-Step "Opening backend logs window"
$backendProcess = Open-PowerShellWindow `
    -Title "Backend .NET logs" `
    -WorkingDirectory $serverPath `
    -Command "dotnet watch run" `
    -LogFilePath $backendLogPath

Write-Host "Backend logs window started. PID: $($backendProcess.Id)" -ForegroundColor Green
Write-Host "Backend log file: $backendLogPath" -ForegroundColor DarkGray

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
Write-Host "Logs directory      : $logsPath"
Write-Host "Frontend log file   : $frontendLogPath"
Write-Host "Press Ctrl + C in a given window to stop its process." -ForegroundColor DarkYellow
Write-Host ""

Start-Transcript -Path $frontendLogPath -Append | Out-Null
try {
    npx ng serve
}
finally {
    Stop-Transcript | Out-Null
}