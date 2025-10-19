using api.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Try to initialize database, but don't fail if database is not available
    // (e.g., during NSwag OpenAPI generation in CI/CD pipeline)
    try
    {
        await app.InitialiseDatabaseAsync();
    }
    catch (Exception)
    {
        // Database not available during build/NSwag generation - this is expected
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogDebug("Database initialization skipped - not available during build");
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});


app.UseExceptionHandler(options => { });

app.Map("/", () => Results.Redirect("/api"));

app.MapEndpoints();

app.Run();

public partial class Program { }
