using GameLib.DAL.Factories;

namespace GameLib.DAL.Tests;

public class DbContextTestsBase : IAsyncLifetime
{
    protected readonly GameLibDbContext GameLibDbContextSut;
    protected readonly DbContextSqLiteFactory DbContextFactory;

    protected DbContextTestsBase()
    {
        DbContextFactory = new DbContextSqLiteFactory(GetType().FullName!);
        GameLibDbContextSut = DbContextFactory.CreateDbContext();
    }

    public async Task InitializeAsync() => await GameLibDbContextSut.Database.EnsureCreatedAsync();

    public async Task DisposeAsync()
    {
        await GameLibDbContextSut.Database.EnsureDeletedAsync();
        await GameLibDbContextSut.DisposeAsync();
    }
}