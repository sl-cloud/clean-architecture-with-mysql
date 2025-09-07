namespace api.Application.FunctionalTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync()
    {
        // Using local PostgreSQL database instead of Docker containers
        // To use Docker containers, switch to `PostgreSQLTestcontainersTestDatabase`
        var database = new PostgreSQLTestDatabase();

        await database.InitialiseAsync();

        return database;
    }
}
