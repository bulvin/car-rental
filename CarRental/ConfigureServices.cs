using System.Reflection;
using CarRental.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CarRental;

public static class ConfigureServices
{
    private static readonly Assembly Assembly = typeof(ConfigureServices).Assembly;

    public static void AddServices(this WebApplicationBuilder builder)
    {
        
        builder.AddDatabase();
        
        builder.Services.AddValidatorsFromAssembly(Assembly);
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly);
        });
    }
    
    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
        });
    }
}