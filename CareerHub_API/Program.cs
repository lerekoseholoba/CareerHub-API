using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Handles unhandled exceptions globally
app.UseExceptionHandler();

// Returns Problem Details for empty 404/400 responses
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

app.Run();