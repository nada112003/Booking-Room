# University Room Booking System

A .NET 8 Web API for managing university room bookings.

## Features

- User Management (Students, Staff, Admins)
- Room Management
- Booking System
- Booking Status Management
- Date Range Queries
- Room Availability Checking

## Project Structure

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or full version)
- Visual Studio 2022 or Visual Studio Code

### Opening the Project

#### Using Visual Studio 2022:
1. Open Visual Studio 2022
2. Click on "Open a project or solution"
3. Navigate to the project folder
4. Select `BookingRoom.sln`
5. Click "Open"

#### Using Visual Studio Code:
1. Open Visual Studio Code
2. Click on "File" > "Open Folder"
3. Navigate to the project folder
4. Select the folder and click "Select Folder"
5. Open a terminal in VS Code (Ctrl + `)
6. Run the following commands:
   ```bash
   dotnet restore
   dotnet build
   dotnet run --project BookingRoom.API
   ```

### Setup

1. Update the connection string in `appsettings.json` if needed
2. Open Package Manager Console in Visual Studio and run:
   ```bash
   Update-Database
   ```
   Or using .NET CLI:
   ```bash
   dotnet ef database update
   ```

### Running the Project

#### Using Visual Studio:
1. Set `BookingRoom.API` as the startup project
2. Press F5 or click the "Start" button

#### Using .NET CLI:
```bash
cd BookingRoom.API
dotnet run
```

The API will be available at:
- https://localhost:7001
- http://localhost:5001

### API Documentation

The API documentation is available at `/swagger` when running in development mode:
- https://localhost:7001/swagger
- http://localhost:5001/swagger

[Rest of the README content remains the same...]