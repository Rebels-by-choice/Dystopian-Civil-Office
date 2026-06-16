## Scripts explaination

`Windows\Run.ps1` - automaticly opens up to 3 powershell windows for real-time logs for containers, backend and frontend.

`Unix\Run.sh` - because there are many linux systems, which have multiple terminal emulators, multiple windows mechanism is not implemented.

In repository root, there will be log files in `\logs` for each application instance. Logs, that are older than 7 days, are automaticly cleared.

### Script pipeline

1. Builds and runs required 4 containers as subservices.
2. Configurates backend data.
3. Runs ASP.NET backend service.
4. Installs dependencies on frontend.
5. Runs Angular frontend service.
