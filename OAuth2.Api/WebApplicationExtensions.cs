using OAuth2.Application.Interfaces.Auth;

namespace OAuth2.Api;

public static class WebApplicationExtensions
{
    public static async Task SeedAdminUserAsync(this WebApplication app)
    {
        var connectionString = app.Configuration["MongoDb:ConnectionString"];
        var databaseName = app.Configuration["MongoDb:DatabaseName"];
        if (string.IsNullOrWhiteSpace(connectionString) ||
            string.IsNullOrWhiteSpace(databaseName))
        {
            return;
        }

        if (connectionString.Contains('<', StringComparison.Ordinal) ||
            databaseName.Contains('<', StringComparison.Ordinal))
        {
            return;
        }

        using var scope = app.Services.CreateScope();
        var userStore = scope.ServiceProvider.GetRequiredService<IUserStore>();

        if (await userStore.AnyAsync(CancellationToken.None))
        {
            return;
        }

        var email = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
        if (string.IsNullOrWhiteSpace(email))
        {
            email = "admin@local";
        }

        var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
        if (string.IsNullOrWhiteSpace(password))
        {
            password = "Admin123!";
        }

        await userStore.CreateAsync(email, password, CancellationToken.None);
    }
}
