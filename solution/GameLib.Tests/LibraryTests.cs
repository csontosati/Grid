using GameLib.Common.Tests;
using GameLib.Common.Tests.Seeds;
using GameLib.DAL.Entities;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GameLib.DAL.Tests;

public class LibraryTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_Library_Persisted()
    {
        var user = UserSeeds.UserEntity;
        GameLibDbContextSut.Users.Add(user);
        await GameLibDbContextSut.SaveChangesAsync();

        var entity = LibrarySeeds.LibraryEntity with
        {
            UserId = user.Id,
            Games = new List<GameEntity>()
        };

        GameLibDbContextSut.Libraries.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        GameLibDbContextSut.ChangeTracker.Clear();
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Libraries.SingleAsync(l => l.Id == entity.Id);

        Assert.Equal(entity.Name, actual.Name);
        Assert.Equal(entity.UserId, actual.UserId);
    }

    [Fact]
    public async Task GetById_Library_Persisted()
    {
        var user = UserSeeds.UserEntity;
        var library = LibrarySeeds.LibraryEntity with { UserId = user.Id, Games = new List<GameEntity>() };
        GameLibDbContextSut.Users.Add(user);
        GameLibDbContextSut.Libraries.Add(library);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Libraries.SingleOrDefaultAsync(l => l.Id == library.Id);

        Assert.NotNull(actual);
        Assert.Equal(library.Name, actual.Name);
        Assert.Equal(library.UserId, actual.UserId);
    }

    [Fact]
    public async Task Update_Library_RemoveGame_Persisted()
    {
        var studio = StudioSeeds.StudioEntity;
        var user = UserSeeds.UserEntity;
        var game = GameSeeds.GameDelete with { StudioId = studio.Id, Studio = null! };
        var library = LibrarySeeds.LibraryEntity2 with { UserId = user.Id, Games = new List<GameEntity> { game } };

        GameLibDbContextSut.Studios.Add(studio);
        GameLibDbContextSut.Users.Add(user);
        GameLibDbContextSut.Games.Add(game);
        GameLibDbContextSut.Libraries.Add(library);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        var libraryToUpdate = await GameLibDbContextSut.Libraries
            .Include(l => l.Games)
            .SingleAsync(l => l.Id == library.Id);

        libraryToUpdate.Games.Clear();
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Libraries.Include(l => l.Games).SingleAsync(l => l.Id == library.Id);

        Assert.Empty(actual.Games);
        Assert.Equal(library.Name, actual.Name);
    }

    [Fact]
    public async Task Delete_Library_Persisted()
    {
        var user = UserSeeds.UserEntityDelete;
        var library = LibrarySeeds.LibraryEntity with { UserId = user.Id, Games = new List<GameEntity>() };

        GameLibDbContextSut.Users.Add(user);
        GameLibDbContextSut.Libraries.Add(library);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        var libraryToDelete = await GameLibDbContextSut.Libraries.SingleAsync(l => l.Id == library.Id);
        GameLibDbContextSut.Libraries.Remove(libraryToDelete);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var exists = await dbx.Libraries.AnyAsync(l => l.Id == library.Id);

        Assert.False(exists);
    }
}