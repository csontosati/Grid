using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Xunit;
using GameLib.Common.Tests;
using GameLib.Common.Tests.Seeds;
using Xunit.Abstractions;

namespace GameLib.DAL.Tests;

public class DbContextGameTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_Game_Persisted()
    {
        // Arrange
        GameEntity entity = GameSeeds.WitcherGame with
        {
            Id = Guid.Parse("6B1677DD-2C66-4C31-8727-64BA87DD6303"),
            StudioId = StudioSeeds.StudioEntity.Id,
            Categories = new List<CategoryEntity>(),
            Libraries = new List<LibraryEntity>(),
            Timer = new List<TimerEntity>()
        };

        // Act
        GameLibDbContextSut.Games.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();
        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games.SingleAsync(g => g.Id == entity.Id);
        DeepAssert.Equal(entity, actual, nameof(GameEntity.Categories), nameof(GameEntity.Libraries), nameof(GameEntity.Timer));
    }

    [Fact]
    public async Task AddNew_Game_With_Categories_Persisted()
    {
        // Arrange 
        GameLibDbContextSut.Categories.Attach(CategorySeeds.ActionCategory);
        GameLibDbContextSut.Categories.Attach(CategorySeeds.MMOCategory);

        GameEntity entity = GameSeeds.WitcherGame with
        {
            Id = Guid.Parse("9682F1D0-B5E7-42D9-9339-DAD1F6921431"),
            Categories = new List<CategoryEntity> { CategorySeeds.ActionCategory, CategorySeeds.MMOCategory },
            Libraries = new List<LibraryEntity>(),
            Timer = new List<TimerEntity>()
        };

        // Act
        GameLibDbContextSut.Games.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games
            .Include(g => g.Categories)
            .SingleAsync(g => g.Id == entity.Id);

        DeepAssert.Equal(entity, actual);
    }

    [Fact]
    public async Task AddNew_Game_With_Libraries_Persisted()
    {
        // Arrange 
        GameEntity entity = GameSeeds.WitcherGame with
        {

            Id = Guid.Parse("944B9394-EECC-4807-BC67-06F59FC32EF5"),
            Libraries = new List<LibraryEntity> { LibrarySeeds.LibraryEntity, LibrarySeeds.LibraryEntity2 }
        };

        // Act
        GameLibDbContextSut.Games.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games
            .Include(g => g.Libraries)
            .SingleAsync(g => g.Id == entity.Id);

        DeepAssert.Equal(entity, actual);
    }

    [Fact]
    public async Task AddNew_Game_With_Timers_Persisted()
    {
        // Arrange
        var game = GameSeeds.WitcherGame;

        var timer1 = new TimerEntity
        {
            Id = Guid.Parse("4A7B1BE1-348B-4685-85B5-A680CA318DDB"),
            GameId = game.Id,
            Time = TimeSpan.FromHours(2),
            Date = new DateTime(2024, 1, 1)
        };

        // Act
        GameLibDbContextSut.Timer.Add(timer1);
        await GameLibDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualGame = await dbx.Games
            .Include(g => g.Timer)
            .SingleAsync(g => g.Id == game.Id);

        DeepAssert.Equal(game, actualGame);
    }

    [Fact]
    public async Task Update_Game_Persisted()
    {
        // Arrange
        var game = GameSeeds.GameUpdate;
        game.Name = "Witcher 3: Wild Hunt";
        game.Age = Pegi.Eighteen;

        // Act
        GameLibDbContextSut.Games.Update(game);
        await GameLibDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games.SingleAsync(g => g.Id == game.Id);

        DeepAssert.Equal(game, actual);
    }

    [Fact]
    public async Task Delete_Game_Deletes_Associated_Timers_Persisted()
    {
        // Arrange
        var game = GameSeeds.GameDelete;
        var timer = new TimerEntity
        {

            Id = Guid.Parse("B80A1958-948A-4BC9-B487-02840B47D444"),
            GameId = game.Id,
            Time = TimeSpan.FromMinutes(30),
            Date = DateTime.Now
        };

        GameLibDbContextSut.Timer.Add(timer);
        await GameLibDbContextSut.SaveChangesAsync();

        // Act
        GameLibDbContextSut.Games.Remove(game);
        await GameLibDbContextSut.SaveChangesAsync();

        // Assert 
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var gameExists = await dbx.Games.AnyAsync(g => g.Id == game.Id);
        var timerExists = await dbx.Timer.AnyAsync(t => t.Id == timer.Id);

        Assert.False(gameExists);
        Assert.False(timerExists);
    }

    [Fact]
    public async Task GetById_Game_Persisted()
    {
        // Act
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games.SingleAsync(g => g.Id == GameSeeds.WitcherGame.Id);

        // Assert
        DeepAssert.Equal(GameSeeds.WitcherGame, actual);
    }

    [Fact]
    public async Task GetById_IncludingCategories_Game_Persisted()
    {
        // Act
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games
            .Include(g => g.Categories)
            .SingleAsync(g => g.Id == GameSeeds.WitcherGame.Id);

        // Assert
        DeepAssert.Equal(GameSeeds.WitcherGame, actual);
    }
}