## Scripts explaination
`Windows\Run.ps1` - automaticly opens up to 3 powershell windows for real-time logs for containers, backend and frontend.

`Linux\Run.sh` - because there are many linux systems, which have multiple terminal emulators, multiple window mechanism is not implemented.

In repository root, there will be log files in `\logs` for each application instance. Logs, that are older than 7 days, are automaticly cleared.

### Script pipeline
1. Builds and runs all 6 containers.
2. Configurates backend data.
3. Runs ASP.NET backend.
4. Installs dependencies on frontend.
5. Runs Angular frontend service.