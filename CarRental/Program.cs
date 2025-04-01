using System.Text.Json.Serialization;
using System.Threading.Channels;
using CarRental;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
        
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "CAR RENTAL API", Version = "v1" });
    o.EnableAnnotations();
});


builder.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFrontendApp");
app.UseHttpsRedirection();
app.MapControllers();
app.UseExceptionHandler();

await app.Configure();
app.Run();