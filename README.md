# api

This project is based on the [Clean.Architecture.Solution.Template](https://github.com/jasontaylordev/CleanArchitecture) version 9.0.12, with the following customizations:

- **Sample data removed** - Stripped down to a clean starting point
- **Database provider** - Configured to use MySQL instead of the template's default SQL Server
- **Azure deployment ready** - Includes Bicep templates for Azure App Service and MySQL deployment

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
cd .\src\Web\
dotnet watch run
```

Navigate to https://localhost:5001. The application will automatically reload if you change any of the source files.

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the coding styles applicable to this solution.

## Code Scaffolding

The template includes support to scaffold new commands and queries.

Start in the `.\src\Application\` folder.

Create a new command:

```
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int
```

Create a new query:

```
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```

If you encounter the error *"No templates or subcommands found matching: 'ca-usecase'."*, install the template and try again:

```bash
dotnet new install Clean.Architecture.Solution.Template::9.0.12
```

## Test

The solution contains unit, integration, and functional tests.

To run the tests:
```bash
dotnet test
```

## Help
To learn more about the template go to the [project website](https://github.com/jasontaylordev/CleanArchitecture). Here you can find additional guidance, request new features, report a bug, and discuss the template with other users.