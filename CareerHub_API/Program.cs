using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using CareerHub_API.Data;
using Serilog;
using CareerHub_API.Middleware;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Return enums as strings instead of numbers
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddSingleton<JobService>();

// Adds RFC 7807 Problem Details support
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

app.UseSerilogRequestLogging();

app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

app.Run();