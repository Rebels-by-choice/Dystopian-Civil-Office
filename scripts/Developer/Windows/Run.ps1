@echo off

:: <Run docker containers>
cd ..\..\..\
docker compose up -d --build
timeout /t 3 /nobreak > nul
echo Docker containers builded correctly

:: <Run backend ASP.NET>
cd apps\server\src\Dystopian-Civil-Office\Dystopian-Civil-Office\
start /b dotnet ef database update
start /b dotnet watch run
echo Backend is running

:: <Run frontend Angular>
cd ..\..\..\..\client
call ng serve
echo Frontend is running

echo.
echo.
echo Press ctrl + C to stop all

pause
