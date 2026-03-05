using GameLib.DAL.Factories;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace GameLib.DAL.Tests;

public class DbContextTestsBase : IAsyncLifetime
{

    protected DbContextTestsBase(ITestOutputHelper output)
    {
        var databaseName = $"{GetType().FullName}_{Guid.NewGuid():N}.db";
        DbContextFactory = new DbContextSqLiteFactory($"Data Source={databaseName}");
        GameLibDbContextSut = DbContextFactory.CreateDbContext();
    }
    protected IDbContextFactory<GameLibDbContext> DbContextFactory { get; }
    protected GameLibDbContext GameLibDbContextSut { get; }

    public async Task InitializeAsync()
    {

        await GameLibDbContextSut.Database.EnsureDeletedAsync();
        await GameLibDbContextSut.Database.EnsureCreatedAsync();


    }

    public async Task DisposeAsync()
    {
        await GameLibDbContextSut.Database.EnsureDeletedAsync();
        await GameLibDbContextSut.DisposeAsync();
    }
}