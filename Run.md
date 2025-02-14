# KeyFortress

An uncompromising and Multi-user console-based password manager built by Dot Net Core and Sqlite3.

## Running in Development

## Using Automated Script

KeyFortress comes baked with automated scripts for development.

- For windows: double click and open `run.bat` and that's it!
- For mac and linux, run `sudo chmod +x run.sh` for the first time and `./run.sh` from then on.

> NOTE: Linux and Mac must have at least dotnet core installed.

## Running it From Scratch

### Prerequisites

- Dot net core must be installed
- Entity Framework Core must be installed.
  - To install it run: `dotnet tool install --global dotnet-ef`

### Install and Run

- Clone repository or download source files.
- Navigate to the project in Console.
- Run `dotnet restore` to install the dependencies.
- Sync the database to the latest changes: `dotnet ef database update`
- Run `dotnet run Program.cs` to explore the application.
