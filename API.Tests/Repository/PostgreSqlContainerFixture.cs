using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Configurations;
using Xunit;

namespace API.Tests.Repository;

public class PostgreSqlContainerFixture : IAsyncLifetime
{
    private readonly PostgreSqlTestcontainer _container;

    public string ConnectionString => _container.ConnectionString;

    public PostgreSqlContainerFixture()
    {
        _container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "careerhub_test",
                Username = "postgres",
                Password = "postgres"
            })
            .WithImage("postgres:16")
            .WithCleanUp(true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}