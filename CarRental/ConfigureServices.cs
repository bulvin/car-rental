using System.Reflection;
using System.Text.Json.Serialization;
using CarRental.Common;
using CarRental.Common.Exceptions;
using CarRental.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace CarRental;

public static class ConfigureServices
{
    private static readonly Assembly Assembly = typeof(ConfigureServices).Assembly;

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.AddSwagger();
        builder.AddDatabase();
        
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        
        builder.Services.AddValidatorsFromAssembly(Assembly);
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        
        builder.AddCors();
    }

    private static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc("v1", new OpenApiInfo { Title = "CAR RENTAL API", Version = "v1" });
            o.EnableAnnotations();
        });
    }
    
    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
        });

        builder.Services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
    }
    
    private static void AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontendApp",
                b =>
                {
                    b
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }
}