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
        GameLibDbContextSut.Studios.Add(StudioSeeds.StudioEntity);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

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
        DeepAssert.Equal(entity, actual);
    }

    [Fact]
    public async Task AddNew_Game_With_Categories_Persisted()
    {
        // Arrange 
        GameLibDbContextSut.Studios.Add(StudioSeeds.StudioEntity);
        GameLibDbContextSut.Categories.AddRange(CategorySeeds.ActionCategory, CategorySeeds.MMOCategory);

        await GameLibDbContextSut.SaveChangesAsync();

        GameEntity entity = GameSeeds.WitcherGame with
        {
            Id = Guid.Parse("9682F1D0-B5E7-42D9-9339-DAD1F6921431"),
            StudioId = StudioSeeds.StudioEntity.Id,
            Categories = new List<CategoryEntity> { CategorySeeds.ActionCategory, CategorySeeds.MMOCategory },
            Libraries = new List<LibraryEntity>(),
            Timer = new List<TimerEntity>()
        };

        // Act
        GameLibDbContextSut.Categories.AttachRange(CategorySeeds.ActionCategory, CategorySeeds.MMOCategory);
        GameLibDbContextSut.Games.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games
            .Include(g => g.Categories)
            .SingleAsync(g => g.Id == entity.Id);

        DeepAssert.Equal(entity, actual, nameof(GameEntity.Libraries), nameof(GameEntity.Timer), nameof(GameEntity.Studio));
    }

    [Fact]
    public async Task AddNew_Game_With_Libraries_Persisted()
    {
        // Arrange
        GameLibDbContextSut.Studios.Add(StudioSeeds.StudioEntity);
        GameLibDbContextSut.Users.Add(UserSeeds.UserEntity);
        GameLibDbContextSut.Libraries.AddRange(LibrarySeeds.LibraryEntity, LibrarySeeds.LibraryEntity2);

        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        GameEntity entity = GameSeeds.WitcherGame with
        {
            Id = Guid.Parse("944B9394-EECC-4807-BC67-06F59FC32EF5"),
            StudioId = StudioSeeds.StudioEntity.Id,
            Libraries = new List<LibraryEntity> { LibrarySeeds.LibraryEntity, LibrarySeeds.LibraryEntity2 },
            Categories = new List<CategoryEntity>(),
            Timer = new List<TimerEntity>()
        };

        // Act
        GameLibDbContextSut.Libraries.AttachRange(LibrarySeeds.LibraryEntity, LibrarySeeds.LibraryEntity2);
        GameLibDbContextSut.Games.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games
            .Include(g => g.Libraries)
            .SingleAsync(g => g.Id == entity.Id);

        Assert.Equal(entity.Libraries.Count, actual.Libraries.Count);

        DeepAssert.Equal(entity, actual,
            nameof(GameEntity.Categories),
            nameof(GameEntity.Timer),
            nameof(GameEntity.Studio),
            nameof(GameEntity.Libraries));
    }

    [Fact]
    public async Task AddNew_Game_With_Timers_Persisted()
    {
        // Arrange
        GameLibDbContextSut.Studios.Add(StudioSeeds.StudioEntity);

        var game = GameSeeds.WitcherGame with
        {
            StudioId = StudioSeeds.StudioEntity.Id,
            Timer = new List<TimerEntity>(),
            Categories = new List<CategoryEntity>(),
            Libraries = new List<LibraryEntity>()
        };
        GameLibDbContextSut.Games.Add(game);

        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

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

        DeepAssert.Equal(game with { Timer = new List<TimerEntity> { timer1 } }, actualGame,
            nameof(GameEntity.Categories),
            nameof(GameEntity.Libraries),
            nameof(GameEntity.Studio));
    }
    [Fact]
    public async Task Update_Game_Persisted()
    {
        // Arrange
        GameLibDbContextSut.Studios.Add(StudioSeeds.StudioEntity);

        var gameToSeed = GameSeeds.GameUpdate with
        {
            StudioId = StudioSeeds.StudioEntity.Id,
            Categories = new List<CategoryEntity>(),
            Libraries = new List<LibraryEntity>(),
            Timer = new List<TimerEntity>()
        };
        GameLibDbContextSut.Games.Add(gameToSeed);
        await GameLibDbContextSut.SaveChangesAsync();

        GameLibDbContextSut.ChangeTracker.Clear();

        var gameToUpdate = gameToSeed with
        {
            Name = "Witcher 3: Wild Hunt",
            Age = Pegi.Eighteen,
            Studio = null! 
        };

        // Act
        GameLibDbContextSut.Games.Update(gameToUpdate);
        await GameLibDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games.SingleAsync(g => g.Id == gameToUpdate.Id);

        DeepAssert.Equal(gameToUpdate, actual,
            nameof(GameEntity.Categories),
            nameof(GameEntity.Libraries),
            nameof(GameEntity.Timer),
            nameof(GameEntity.Studio));
    }

    [Fact]
    public async Task Delete_Game_Deletes_Associated_Timers_Persisted()
    {
        // Arrange
        GameLibDbContextSut.Studios.Add(StudioSeeds.StudioEntity);

        var game = GameSeeds.GameDelete with
        {
            StudioId = StudioSeeds.StudioEntity.Id,
            Studio = null!,
            Timer = new List<TimerEntity>(),
            Categories = new List<CategoryEntity>(),
            Libraries = new List<LibraryEntity>()
        };
        GameLibDbContextSut.Games.Add(game);

        var timer = new TimerEntity
        {
            Id = Guid.Parse("B80A1958-948A-4BC9-B487-02840B47D444"),
            GameId = game.Id,
            Time = TimeSpan.FromMinutes(30),
            Date = DateTime.Now
        };
        GameLibDbContextSut.Timer.Add(timer);

        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        // Act
        var gameToDelete = await GameLibDbContextSut.Games
            .Include(g => g.Timer)
            .SingleAsync(g => g.Id == game.Id);

        GameLibDbContextSut.Games.Remove(gameToDelete);
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
        // Arrange
        GameLibDbContextSut.Studios.Add(StudioSeeds.StudioEntity);

        var gameToSeed = GameSeeds.WitcherGame with
        {
            StudioId = StudioSeeds.StudioEntity.Id,
            Studio = null!,
            Categories = new List<CategoryEntity>(),
            Libraries = new List<LibraryEntity>(),
            Timer = new List<TimerEntity>()
        };
        GameLibDbContextSut.Games.Add(gameToSeed);
        await GameLibDbContextSut.SaveChangesAsync();

        GameLibDbContextSut.ChangeTracker.Clear();

        // Act
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games.SingleAsync(g => g.Id == gameToSeed.Id);

        // Assert
        DeepAssert.Equal(gameToSeed, actual,
            nameof(GameEntity.Categories),
            nameof(GameEntity.Libraries),
            nameof(GameEntity.Timer),
            nameof(GameEntity.Studio));
    }

    [Fact]
    public async Task GetById_IncludingCategories_Game_Persisted()
    {
        // Arrange
        GameLibDbContextSut.Studios.Add(StudioSeeds.StudioEntity);
        GameLibDbContextSut.Categories.AddRange(CategorySeeds.ActionCategory, CategorySeeds.MMOCategory);
        await GameLibDbContextSut.SaveChangesAsync();

        var gameToSeed = GameSeeds.WitcherGame with
        {
            StudioId = StudioSeeds.StudioEntity.Id,
            Studio = null!,
            Categories = new List<CategoryEntity> { CategorySeeds.ActionCategory, CategorySeeds.MMOCategory },
            Libraries = new List<LibraryEntity>(),
            Timer = new List<TimerEntity>()
        };

        GameLibDbContextSut.Games.Add(gameToSeed);
        await GameLibDbContextSut.SaveChangesAsync();

        GameLibDbContextSut.ChangeTracker.Clear();

        // Act
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games
            .Include(g => g.Categories)
            .SingleAsync(g => g.Id == gameToSeed.Id);

        // Assert
        DeepAssert.Equal(gameToSeed, actual,
            nameof(GameEntity.Libraries),
            nameof(GameEntity.Timer),
            nameof(GameEntity.Studio));
    }
}