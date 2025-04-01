using System.Text.Json.Serialization;
using CarRental;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
        
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseExceptionHandler();
await app.Configure();
app.Run();