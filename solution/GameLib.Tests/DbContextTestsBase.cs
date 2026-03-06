using GameLib.DAL.Factories;
using GameLib.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
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

        await SeedAsync();
    }

    private async Task SeedAsync()
    {
        UserSeeds.SeedUsers(GameLibDbContextSut);
        StudioSeeds.SeedStudios(GameLibDbContextSut);
        CategorySeeds.SeedCategories(GameLibDbContextSut);
        await GameLibDbContextSut.SaveChangesAsync();

        GameLibDbContextSut.ChangeTracker.Clear();

        GameSeeds.SeedGames(GameLibDbContextSut);
        await GameLibDbContextSut.SaveChangesAsync();

        GameLibDbContextSut.ChangeTracker.Clear();

        LibrarySeeds.SeedLibraries(GameLibDbContextSut);
        await GameLibDbContextSut.SaveChangesAsync();

        GameLibDbContextSut.ChangeTracker.Clear();
    }
    public async Task DisposeAsync()
    {
        await GameLibDbContextSut.Database.EnsureDeletedAsync();
        await GameLibDbContextSut.DisposeAsync();
    }
}