using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using CareerHub_API.Data;
using CareerHub_API.Middleware;
using CareerHub_API.Infrastructure;
using CareerHub_API.Infrastructure.OpenApi;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

// ----------------------
// API Versioning
// ----------------------
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// ----------------------
// Register Services
// ----------------------
builder.Services.AddAuthServices();
builder.Services.AddJobServices();
builder.Services.AddApplicationServices();
builder.Services.AddScoped<CareerHubDocumentTransformer>();
builder.Services.AddOpenApi(options=>
  {
    options.AddDocumentTransformer<CareerHubDocumentTransformer>();
  } 
);

// ----------------------
// Problem Details / Exceptions
// ----------------------
builder.Services.AddProblemDetails();
//builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// ----------------------
// CORS
// ----------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000",
                "https://careerhub.com")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("X-Total-Count");
    });
});

// ----------------------
// JWT Authentication
// ----------------------
var jwtSecretKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(jwtSecretKey))
{
    throw new InvalidOperationException(
        "JWT Secret key is not configured");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSecretKey))
            };
    });

builder.Services.AddAuthorization();

// ----------------------
// Rate Limiting
// ----------------------
builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;

        if (context.Lease.TryGetMetadata(
            MetadataName.RetryAfter,
            out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter =
                ((int)retryAfter.TotalSeconds).ToString();

            await context.HttpContext.Response.WriteAsync(
                $"Rate limit exceeded. Please retry after {(int)retryAfter.TotalSeconds} seconds.",
                token);
        }
        else
        {
            await context.HttpContext.Response.WriteAsync(
                "Rate limit exceeded.",
                token);
        }
    };

    // Global Policy
    options.AddFixedWindowLimiter(
        "global",
        limiterOptions =>
        {
            limiterOptions.PermitLimit = 200;
            limiterOptions.Window = TimeSpan.FromSeconds(60);
            limiterOptions.QueueLimit = 0;
        });

    // Search Policy
    options.AddSlidingWindowLimiter(
        "search",
        limiterOptions =>
        {
            limiterOptions.PermitLimit = 30;
            limiterOptions.Window = TimeSpan.FromSeconds(60);
            limiterOptions.SegmentsPerWindow = 6;
            limiterOptions.QueueLimit = 0;
        });

    // Apply Policy
    options.AddFixedWindowLimiter(
        "apply",
        limiterOptions =>
        {
            limiterOptions.PermitLimit = 5;
            limiterOptions.Window = TimeSpan.FromHours(1);
            limiterOptions.QueueLimit = 0;
        });

    // Post Listing Policy
    options.AddFixedWindowLimiter(
        "post-listing",
        limiterOptions =>
        {
            limiterOptions.PermitLimit = 10;
            limiterOptions.Window = TimeSpan.FromHours(1);
            limiterOptions.QueueLimit = 0;
        });
});
//Response Compression
 builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
        options.MimeTypes = ResponseCompressionDefaults.MimeTypes
            .Append("application/json");
    });
//Database health checks
 builder.Services.AddHealthChecks()
        .AddDbContextCheck<CareerHubDbContext>(
            name: "database",
            failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
            tags: ["ready"]);

// ----------------------
// DbContext
// ----------------------
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CareerHubDbContext>(options =>
{
    if (connectionString!.Contains("Host="))
    {
        options.UseNpgsql(connectionString);
    }
    else
    {
        options.UseSqlite(connectionString);
    }

    options.EnableSensitiveDataLogging()
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

app.UseResponseCompression();

app.UseCors("Frontend");

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();

app.UseStatusCodePages();

// ----------------------
// Development Only
// ----------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// ----------------------
// Controllers
// ----------------------
app.MapControllers()
   .RequireRateLimiting("global");

app.MapHealthChecks("/health/live", new HealthCheckOptions
    {
        Predicate = _ => false
    });

app.MapHealthChecks("/health/ready", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("ready")
    });

// ----------------------
// Run
// ----------------------
app.Run();
public partial class Program
{
}
