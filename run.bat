@echo off
REM Check if dotnet is installed
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo Error: .NET Core is not installed. Please install it and try again.
    exit /b 1
)

REM Check if Entity Framework Core is installed
dotnet ef --version >nul 2>&1
if errorlevel 1 (
    echo Error: Entity Framework Core is not installed. Installing...
    dotnet tool install --global dotnet-ef
    if errorlevel 1 (
        echo Error: Failed to install Entity Framework Core. Please try again manually.
        exit /b 1
    )
)

REM Clone repository or download source files
REM Navigate to the project directory

REM Run dotnet restore
echo Restoring dependencies...
dotnet restore
if errorlevel 1 (
    echo Error: Failed to restore dependencies.
    exit /b 1
)

REM Sync the database to the latest changes
echo Syncing the database to the latest changes...
dotnet ef database update
if errorlevel 1 (
    echo Error: Failed to sync the database to the latest changes.
    exit /b 1
)

REM Run the application
echo Running the application...
dotnet run Program.cs
if errorlevel 1 (
    echo Error: Failed to run the application.
    exit /b 1
)

exit /b 0
