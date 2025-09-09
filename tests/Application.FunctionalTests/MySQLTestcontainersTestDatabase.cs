using System.Data.Common;
using api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Respawn;
using Testcontainers.MySql;

namespace api.Application.FunctionalTests;

public class MySQLTestcontainersTestDatabase : ITestDatabase
{
    private const string DefaultDatabase = "apiTestDb";
    private readonly MySqlContainer _container;
    private DbConnection _connection = null!;
    private string _connectionString = null!;
    private Respawner _respawner = null!;

    public MySQLTestcontainersTestDatabase()
    {
        _container = new MySqlBuilder()
            .WithDatabase(DefaultDatabase)
            .WithUsername("root")
            .WithPassword("adminpass")
            .WithAutoRemove(true)
            .Build();
    }

    public async Task InitialiseAsync()
    {
        await _container.StartAsync();

        _connectionString = _container.GetConnectionString();

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
        await _container.DisposeAsync();
    }
}
