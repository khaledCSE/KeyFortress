#!/bin/bash
set -e

# Check if dotnet is installed
if ! command -v dotnet &> /dev/null; then
    echo "Error: .NET Core is not installed. Please install it and try again."
    exit 1
fi

# Check if Entity Framework Core is installed
if ! dotnet tool list -g | grep -q dotnet-ef; then
    echo "Error: Entity Framework Core is not installed. Installing..."
    dotnet tool install --global dotnet-ef
    if [ $? -ne 0 ]; then
        echo "Error: Failed to install Entity Framework Core. Please try again manually."
        exit 1
    fi
fi

# Clone repository or download source files
# Navigate to the project directory

# Run dotnet restore
echo "Restoring dependencies..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "Error: Failed to restore dependencies."
    exit 1
fi

# Sync the database to the latest changes
echo "Syncing the database to the latest changes..."
dotnet ef database update
if [ $? -ne 0 ]; then
    echo "Error: Failed to sync the database to the latest changes."
    exit 1
fi

# Run the application
echo "Running the application..."
dotnet run Program.cs
if [ $? -ne 0 ]; then
    echo "Error: Failed to run the application."
    exit 1
fi

exit 0
