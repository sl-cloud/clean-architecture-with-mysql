namespace api.Application.FunctionalTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync()
    {
        // Using local MySQL database (same as development environment)
        // Make sure MySQL is running locally with the correct user credentials
        var database = new MySQLTestDatabase();

        await database.InitialiseAsync();

        return database;
    }
}
