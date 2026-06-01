using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using CareerHub_API.Data;
using Serilog;
using CareerHub_API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
//Register Services
builder.Services.AddSingleton<JobService>();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddCors(options =>
    {
     options.AddPolicy("FrontEndPolicy", policy =>
     {
        policy.WithOrigins("http://localhost:3000") // only sites with this origin can call this API
        .AllowAnyHeader() //Allows authorization, Content-Type, etc
        .AllowAnyMethod(); //Allows GET,POST,DELETE etc.. 
     }); 
    });
var jwtSecretKey = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Not validating who issues it bc its our own API
            ValidateAudience = false, // Not checking who it is intended for
            ValidateLifetime = true, // This ensures you are able to reject expired tokens
            ValidateIssuerSigningKey = true,// verify the signature
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecretKey)
            )
        };
    });
builder.Services.AddAuthorization();
var app = builder.Build();


//Add to Middleware Pipeline
app.UseSerilogRequestLogging();
app.UseCors("FrontEndPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.UseStatusCodePages();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.MapControllers();
app.Run();