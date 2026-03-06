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
    private async Task EnsureStudioExists()
    {
        if (!await GameLibDbContextSut.Studios.AnyAsync(s => s.Id == StudioSeeds.StudioEntity.Id))
        {
            GameLibDbContextSut.Studios.Add(StudioSeeds.StudioEntity);
            await GameLibDbContextSut.SaveChangesAsync();
        }
        GameLibDbContextSut.ChangeTracker.Clear();
    }

    [Fact]
    public async Task AddNew_Game_Persisted()
    {
        await EnsureStudioExists();

        GameEntity entity = GameSeeds.TestGame with
        {
            Id = Guid.Parse("6B1677DD-2C66-4C31-8727-64BA87DD6303"),
            StudioId = StudioSeeds.StudioEntity.Id,
            Categories = new List<CategoryEntity>(),
            Libraries = new List<LibraryEntity>(),
            Timer = new List<TimerEntity>()
        };

        GameLibDbContextSut.Games.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games.SingleAsync(g => g.Id == entity.Id);
        DeepAssert.Equal(entity, actual);
    }

    [Fact]
    public async Task AddNew_Game_With_Categories_Persisted()
    {
        await EnsureStudioExists();
       
        GameLibDbContextSut.Categories.AddRange(CategorySeeds.ActionCategory, CategorySeeds.MMOCategory);
        await GameLibDbContextSut.SaveChangesAsync();
        
        GameLibDbContextSut.ChangeTracker.Clear();

        GameEntity entity = GameSeeds.GameWithCategories with
        {
            StudioId = StudioSeeds.StudioEntity.Id,
            Categories = new List<CategoryEntity>(),
            Libraries = new List<LibraryEntity>(),
            Timer = new List<TimerEntity>()
        };

        GameLibDbContextSut.Games.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        var cat1 = await GameLibDbContextSut.Categories.SingleAsync(c => c.Id == CategorySeeds.ActionCategory.Id);
        var cat2 = await GameLibDbContextSut.Categories.SingleAsync(c => c.Id == CategorySeeds.MMOCategory.Id);

        entity.Categories.Add(cat1);
        entity.Categories.Add(cat2);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games
            .Include(g => g.Categories)
            .SingleAsync(g => g.Id == entity.Id);

        DeepAssert.Equal(entity, actual);
    }

    [Fact]
    public async Task AddNew_Game_With_Libraries_Persisted()
    {
        await EnsureStudioExists();
        
        GameLibDbContextSut.Users.Add(UserSeeds.UserEntity);
        await GameLibDbContextSut.SaveChangesAsync();
        
        GameLibDbContextSut.Libraries.AddRange(
            LibrarySeeds.LibraryEntity with { Games = new List<GameEntity>()},
            LibrarySeeds.LibraryEntity2 with { Games = new List<GameEntity>()});

        await GameLibDbContextSut.SaveChangesAsync();
 
        GameLibDbContextSut.ChangeTracker.Clear();

        GameEntity entity = GameSeeds.GameWithLibraries with
        {
            StudioId = StudioSeeds.StudioEntity.Id,
            Libraries = new List<LibraryEntity>(),
            Categories = new List<CategoryEntity>(),
            Timer = new List<TimerEntity>()
        };

        GameLibDbContextSut.Games.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        var lib1 = await GameLibDbContextSut.Libraries.SingleAsync(l => l.Id == LibrarySeeds.LibraryEntity.Id);
        var lib2 = await GameLibDbContextSut.Libraries.SingleAsync(l => l.Id == LibrarySeeds.LibraryEntity2.Id);

        entity.Libraries.Add(lib1);
        entity.Libraries.Add(lib2);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games
            .Include(g => g.Libraries)
            .SingleAsync(g => g.Id == entity.Id);

        Assert.Equal(2, actual.Libraries.Count);
        DeepAssert.Equal(entity, actual,
            nameof(GameEntity.Categories),
            nameof(GameEntity.Timer),
            nameof(GameEntity.Studio));
    }

    [Fact]
    public async Task AddNew_Game_With_Multiple_Timers_Persisted()
    {
        await EnsureStudioExists();

        var timers = new List<TimerEntity>
        {
            new()
            {
                Id = Guid.Parse("4A7B1BE1-348B-4685-85B5-A680CA318DDB"),
                Time = TimeSpan.FromHours(2),
                Date = new DateTime(2024, 1, 1)
            },
            new()
            {
                Id = Guid.Parse("7D9E2AF2-569C-4796-96C6-B791DB429EEC"),
                Time = TimeSpan.FromMinutes(45),
                Date = new DateTime(2024, 1, 2)
            }
        };

        var game = GameSeeds.TestGame with
        {
            Id = Guid.Parse("A33B9394-EECC-4807-BC67-06F59FC32EF5"),
            StudioId = StudioSeeds.StudioEntity.Id,
            Timer = timers,
            Categories = new List<CategoryEntity>(),
            Libraries = new List<LibraryEntity>()
        };

        GameLibDbContextSut.Games.Add(game);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualGame = await dbx.Games
            .Include(g => g.Timer)
            .SingleAsync(g => g.Id == game.Id);

        DeepAssert.Equal(game, actualGame,
            nameof(GameEntity.Categories),
            nameof(GameEntity.Libraries),
            nameof(GameEntity.Studio));

        Assert.Equal(2, actualGame.Timer.Count);
    }

    [Fact]
    public async Task Update_Game_Persisted()
    {
        await EnsureStudioExists();

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
            Name = "Updated Game",
            Age = Pegi.Eighteen,
            Studio = null!
        };

        GameLibDbContextSut.Games.Update(gameToUpdate);
        await GameLibDbContextSut.SaveChangesAsync();

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
        await EnsureStudioExists();

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

        var gameToDelete = await GameLibDbContextSut.Games
            .Include(g => g.Timer)
            .SingleAsync(g => g.Id == game.Id);

        GameLibDbContextSut.Games.Remove(gameToDelete);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var gameExists = await dbx.Games.AnyAsync(g => g.Id == game.Id);
        var timerExists = await dbx.Timer.AnyAsync(t => t.Id == timer.Id);

        Assert.False(gameExists);
        Assert.False(timerExists);
    }

    [Fact]
    public async Task GetById_Game_Persisted()
    {
        await EnsureStudioExists();

        var gameToSeed = GameSeeds.TestGame with
        {
            Id = Guid.Parse("F11B9394-EECC-4807-BC67-06F59FC32EF5"),
            StudioId = StudioSeeds.StudioEntity.Id,
            Studio = null!,
            Categories = new List<CategoryEntity>(),
            Libraries = new List<LibraryEntity>(),
            Timer = new List<TimerEntity>()
        };
        GameLibDbContextSut.Games.Add(gameToSeed);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games.SingleAsync(g => g.Id == gameToSeed.Id);

        DeepAssert.Equal(gameToSeed, actual,
            nameof(GameEntity.Categories),
            nameof(GameEntity.Libraries),
            nameof(GameEntity.Timer),
            nameof(GameEntity.Studio));
    }

    [Fact]
    public async Task GetById_IncludingCategories_Game_Persisted()
    {
        await EnsureStudioExists();
        if (!await GameLibDbContextSut.Categories.AnyAsync(c => c.Id == CategorySeeds.ActionCategory.Id))
        {
            GameLibDbContextSut.Categories.AddRange(CategorySeeds.ActionCategory, CategorySeeds.MMOCategory);
            await GameLibDbContextSut.SaveChangesAsync();
        }
        GameLibDbContextSut.ChangeTracker.Clear();

        var gameToSeed = GameSeeds.TestGame with
        {
            Id = Guid.Parse("E22B9394-EECC-4807-BC67-06F59FC32EF5"),
            StudioId = StudioSeeds.StudioEntity.Id,
            Studio = null!,
            Categories = new List<CategoryEntity>(),
            Libraries = new List<LibraryEntity>(),
            Timer = new List<TimerEntity>()
        };

        GameLibDbContextSut.Games.Add(gameToSeed);
        await GameLibDbContextSut.SaveChangesAsync();

        var cat1 = await GameLibDbContextSut.Categories.SingleAsync(c => c.Id == CategorySeeds.ActionCategory.Id);
        var cat2 = await GameLibDbContextSut.Categories.SingleAsync(c => c.Id == CategorySeeds.MMOCategory.Id);

        gameToSeed.Categories.Add(cat1);
        gameToSeed.Categories.Add(cat2);
        await GameLibDbContextSut.SaveChangesAsync();

        GameLibDbContextSut.ChangeTracker.Clear();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games
            .Include(g => g.Categories)
            .SingleAsync(g => g.Id == gameToSeed.Id);

        DeepAssert.Equal(gameToSeed, actual,
            nameof(GameEntity.Libraries),
            nameof(GameEntity.Timer),
            nameof(GameEntity.Studio));
    }
}