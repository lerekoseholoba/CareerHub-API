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

//Register Services
builder.Services.AddSingleton<JobService>();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddCors(options =>
    {
     options.AddPolicy("FrontEndPolicy", policy =>
     {
        policy.WithOrigins("http://localhost:3000") // front end dev port
        .AllowAnyHeader() //Allows authorization, Content-Type, etc
        .AllowAnyMethod(); //Allows GET,POST,DELETE etc.. 
     }); 
    });

var app = builder.Build();


//Configure Middleware
app.UseSerilogRequestLogging();
app.UseCors("FrontEndPolicy");
app.UseExceptionHandler();
app.UseStatusCodePages();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.MapControllers();
app.Run();