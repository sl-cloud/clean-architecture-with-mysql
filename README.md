# Clean Architecture with MySQL

This project is based on the [Clean.Architecture.Solution.Template](https://github.com/jasontaylordev/CleanArchitecture) version 9.0.12, with the following customizations:

- **Database provider** - Configured to use MySQL instead of the template's default SQL Server
- **Azure deployment ready** - Includes Bicep templates for Azure App Service and MySQL Flexible Server deployment
- **Docker Compose** - Local MySQL and phpMyAdmin setup for development
- **Sample TodoList API** - Includes basic CQRS examples with TodoLists and TodoItems entities

## Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Git](https://git-scm.com/downloads)
- [Docker](https://www.docker.com/get-started) and Docker Compose

### Clone the Repository

```bash
git clone <repository-url>
cd clean-architecture-with-mysql
```

### MySQL Setup

This project uses MySQL as its database. The easiest way to set up MySQL locally is using Docker:

1. **Start MySQL using Docker Compose:**

```bash
docker compose -f docker/docker-compose.yml up -d
```

This will start:
- MySQL server on `localhost:3306`
- phpMyAdmin on `http://localhost:8081` (optional, for database management)

2. **Create the application database user:**

```bash
docker exec -it mysql mysql -uroot -padminpass -e "CREATE USER IF NOT EXISTS 'appuser'@'%' IDENTIFIED BY 'apppassword'; GRANT ALL PRIVILEGES ON appdb.* TO 'appuser'@'%'; FLUSH PRIVILEGES;"
```

3. **Run the application:**

```bash
cd src/Web
dotnet watch run
```

The application will automatically create the database schema on first run.

**Access the application:**
- **Swagger UI / API Documentation:** https://localhost:5001/api
- **Health Check:** https://localhost:5001/health
- **phpMyAdmin (Database Management):** http://localhost:8081

## Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Git](https://git-scm.com/downloads)
- [Docker](https://www.docker.com/get-started) and Docker Compose

### Clone the Repository

```bash
git clone <repository-url>
cd api_base_mysql
```

### MySQL Setup

This project uses MySQL as its database. The easiest way to set up MySQL locally is using Docker:

1. **Start MySQL using Docker Compose:**

```bash
docker compose -f docker/docker-compose.yml up -d
```

This will start:
- MySQL server on `localhost:3306`
- phpMyAdmin on `http://localhost:8081` (optional, for database management)

2. **Create the application database user:**

```bash
docker exec -it mysql mysql -uroot -padminpass -e "CREATE USER IF NOT EXISTS 'appuser'@'%' IDENTIFIED BY 'apppassword'; GRANT ALL PRIVILEGES ON appdb.* TO 'appuser'@'%'; FLUSH PRIVILEGES;"
```

3. **Run the application:**

```bash
cd src/Web
dotnet watch run
```

The application will automatically create the database schema on first run. Navigate to https://localhost:5001 to access the application.

## Build

Run `dotnet build -tl` to build the solution.

## Run

To run the web application:

```bash
cd src/Web
dotnet watch run
```

Navigate to https://localhost:5001/api for the Swagger UI. The application will automatically reload if you change any of the source files.

## Project Structure

The solution follows Clean Architecture principles with clear separation of concerns:

- **`src/Domain/`** - Enterprise business rules and entities (e.g., TodoItem, TodoList)
- **`src/Application/`** - Application business rules, CQRS commands/queries, interfaces
- **`src/Infrastructure/`** - External concerns (database, identity, file system)
- **`src/Web/`** - API layer with minimal endpoints, middleware, and services
- **`tests/`** - Unit, integration, and functional tests

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the coding styles applicable to this solution.

## Test

The solution contains unit, integration, and functional tests.

To run the tests:
```bash
dotnet test
```

## Azure Deployment

The project includes complete Azure infrastructure as code using Bicep templates for deploying to Azure.

### Prerequisites

- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Azure Developer CLI (azd)](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/install-azd)

### Deploy to Azure

1. **Login to Azure:**

```bash
azd auth login
```

2. **Deploy the application:**

```bash
azd up
```

This will provision:
- Azure App Service for hosting the Web API
- Azure MySQL Flexible Server for the database
- Azure Key Vault for secure configuration storage
- Application Insights for monitoring
- Log Analytics workspace

The connection strings and secrets are automatically stored in Key Vault and configured for the App Service.

## Architecture

This solution implements Clean Architecture with the following patterns:

- **CQRS** - Command Query Responsibility Segregation using MediatR
- **Domain Events** - Domain-driven design with event handlers
- **Repository Pattern** - Data access abstraction via Entity Framework Core
- **Dependency Injection** - Built-in .NET dependency injection
- **Minimal APIs** - Modern .NET endpoint routing

## Help

To learn more about the original Clean Architecture template, go to the [project website](https://github.com/jasontaylordev/CleanArchitecture). Here you can find additional guidance, request new features, report a bug, and discuss the template with other users.