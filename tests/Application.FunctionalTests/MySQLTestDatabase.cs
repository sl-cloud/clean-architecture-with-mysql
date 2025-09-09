using System.Data.Common;
using api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Respawn;

namespace api.Application.FunctionalTests;

public class MySQLTestDatabase : ITestDatabase
{
    private readonly string _connectionString = null!;
    private MySqlConnection _connection = null!;
    private Respawner _respawner = null!;

    public MySQLTestDatabase()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables("TEST_")
            .Build();

        var connectionString = configuration.GetConnectionString("apiDb");

        Guard.Against.Null(connectionString, message: "Connection string 'apiDb' not found in appsettings.json");

        _connectionString = connectionString;
    }

    public async Task InitialiseAsync()
    {
        _connection = new MySqlConnection(_connectionString);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseMySql(_connectionString, GetServerVersion(_connectionString))
            .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning))
            .Options;

        var context = new ApplicationDbContext(options);

        // Only ensure database exists, don't delete it (requires fewer permissions)
        await context.Database.EnsureCreatedAsync();

        await _connection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.MySql
        });
        await _connection.CloseAsync();
    }

    public DbConnection GetConnection()
    {
        return _connection;
    }

    public string GetConnectionString()
    {
        return _connectionString;
    }

    public async Task ResetAsync()
    {
        await _connection.OpenAsync();
        await _respawner.ResetAsync(_connection);
        await _connection.CloseAsync();
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }

    private static ServerVersion GetServerVersion(string connectionString)
    {
        // Use the same approach as the main application - try auto-detect with fallback
        try
        {
            return ServerVersion.AutoDetect(connectionString);
        }
        catch
        {
            // Fallback to a known server version when auto-detection fails (same as development)
            return ServerVersion.Create(8, 0, 0, Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);
        }
    }
}
