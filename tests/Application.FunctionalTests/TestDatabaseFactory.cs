namespace api.Application.FunctionalTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync()
    {
        // Using local MySQL database instead of Docker containers
        // To use Docker containers, switch to `MySQLTestcontainersTestDatabase`
        var database = new MySQLTestDatabase();

        await database.InitialiseAsync();

        return database;
    }
}
