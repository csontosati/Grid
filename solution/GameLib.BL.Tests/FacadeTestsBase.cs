using GameLib.BL.Mappers;
using GameLib.DAL;
using GameLib.DAL.Factories;
using GameLib.DAL.UnitOfWork;
using GameLib.DAL.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GameLib.BL.Tests;

public abstract class FacadeTestsBase : IAsyncLifetime
{
    private readonly ServiceProvider _serviceProvider;

    protected FacadeTestsBase(ITestOutputHelper output)
    {
        var databaseName = $"{GetType().FullName}_{Guid.NewGuid():N}.db";
        DbContextFactory = new DbContextSqLiteFactory($"Data Source={databaseName}");

        LibraryMapper = new LibraryModelMapper();
        GameMapper = new GameModelMapper();
        UserMapper = new UserModelMapper(LibraryMapper);

        var services = new ServiceCollection();

        services.AddSingleton<GameEntityMapper>();
        services.AddSingleton<UserEntityMapper>();
        services.AddSingleton<LibraryEntityMapper>();

        _serviceProvider = services.BuildServiceProvider();

        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory, _serviceProvider);
    }

    protected IDbContextFactory<GameLibDbContext> DbContextFactory { get; }
    protected UnitOfWorkFactory UnitOfWorkFactory { get; }

    protected GameModelMapper GameMapper { get; }
    protected LibraryModelMapper LibraryMapper { get; }
    protected UserModelMapper UserMapper { get; }

    public async Task InitializeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
        await dbx.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
        await _serviceProvider.DisposeAsync();
    }
}