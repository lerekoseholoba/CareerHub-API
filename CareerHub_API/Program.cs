using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using CareerHub_API.Data;
using CareerHub_API.Middleware;
using CareerHub_API.Infrastructure; // <-- for our ServiceCollectionExtensions
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// ----------------------
// Controllers
// ----------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// ----------------------
// Register Services (via extension methods, not direct AddScoped)
// ----------------------
builder.Services.AddAuthServices();
builder.Services.AddJobServices();
builder.Services.AddApplicationServices();

// ----------------------
// Problem Details / Exceptions / Middleware
// ----------------------
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// ----------------------
// CORS
// ----------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ----------------------
// JWT Authentication
// ----------------------
var jwtSecretKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtSecretKey))
    throw new InvalidOperationException("JWT Secret key is not configured");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
        };
    });

builder.Services.AddAuthorization();

// ----------------------
// DbContext
// ----------------------
builder.Services.AddDbContext<CareerHubDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information);
});

// ----------------------
// Build App
// ----------------------
var app = builder.Build();

// ----------------------
// Middleware Pipeline
// ----------------------
app.UseSerilogRequestLogging();
app.UseCors("FrontEndPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.UseStatusCodePages();

// ----------------------
// Dev-only tools
// ----------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// ----------------------
// Controllers
// ----------------------
app.MapControllers();

app.Run();