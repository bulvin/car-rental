using CarRental.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental;

public static class ConfigureApp
{
    public static async Task Configure(this WebApplication app)
    {
        await app.EnsureDatabaseCreated();
    }
    private static async Task EnsureDatabaseCreated(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }
}