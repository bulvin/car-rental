using CarRental.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental;

public static class ConfigureServices
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.AddDatabase();
    }

    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
        });
    }
}