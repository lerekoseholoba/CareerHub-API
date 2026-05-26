using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ========================
// SERVICES (MUST BE BEFORE BUILD)
// ========================
builder.Services.AddControllers();
builder.Services.AddSingleton<JobService>();

// OpenAPI + Scalar
builder.Services.AddOpenApi();

var app = builder.Build();

// ========================
// PIPELINE (AFTER BUILD)
// ========================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

app.Run();


// ========================
// KEEP WEATHER SAMPLE (OPTIONAL)
// ========================
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild",
    "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
});

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}