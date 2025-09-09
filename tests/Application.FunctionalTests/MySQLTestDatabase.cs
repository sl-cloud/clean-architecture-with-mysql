using System;
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
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables("TEST_")
            .Build();

        var connectionString = configuration.GetConnectionString("apiDb");

        Guard.Against.Null(connectionString);

        var builder = new MySqlConnectionStringBuilder(connectionString);

        if (string.IsNullOrWhiteSpace(builder.Password))
        {
            builder.Password = "adminpass";
        }

        _connectionString = builder.ConnectionString;
    }

    public async Task InitialiseAsync()
    {
        Guard.Against.False(
            _connectionString.Contains("Password=", StringComparison.OrdinalIgnoreCase),
            "Database connection string must contain a password.");

        _connection = new MySqlConnection(_connectionString);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString))
            .ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning))
            .Options;

        var context = new ApplicationDbContext(options);

        await context.Database.EnsureDeletedAsync();
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
}
