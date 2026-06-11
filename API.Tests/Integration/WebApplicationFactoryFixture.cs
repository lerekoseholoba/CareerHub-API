using CareerHub_API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace API.Tests.Integration;

public class WebApplicationFactoryFixture
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Intentionally empty.
            // Integration tests use the application's
            // normal configured services.
        });
    }
}