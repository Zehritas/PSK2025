# Github repository for PSK course

# .NET Aspire Project

## Overview
This project is built using **.NET Aspire**, a cloud-native development stack for building distributed applications. It leverages .NET technologies to provide scalable, resilient, and high-performance solutions.

## Prerequisites
Before running this project, ensure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started) (optional, for containerized deployment)

## Getting Started
### Clone the Repository
```sh
git clone https://github.com/Zehritas/PSK2025
cd PSK2025
```

## Run with .NET Aspire

The application is designed to run with .NET Aspire, which coordinates all services, including:
- API service
- Frontend application

From the root directory, run:

```bash
dotnet build
dotnet run --project PSK2025.AppHost
```

This command will:

- Start the API service
- Build and run the frontend

The Aspire dashboard will open in your browser, showing the status of all services. You can access the frontend application and API from there.

## Project Structure

The solution consists of multiple projects:

- **PSK2025.ApiService**: Backend API service
- **PSK2025.AppHost**: Aspire project orchestrator
- **PSK2025.ServiceDefaults**: Shared service configuration
- **PSK2025.Web**: *** frontend
- **PSK2025.Tests**: Test project


