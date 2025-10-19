namespace api.Application.FunctionalTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync()
    {
        // Use Testcontainers - automatically starts MySQL container during tests
        // Works both locally and in CI/CD without manual Docker setup
        var database = new MySQLTestcontainersTestDatabase();

        await database.InitialiseAsync();

        return database;
    }
}
