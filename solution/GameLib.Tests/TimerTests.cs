using GameLib.Common.Tests;
using GameLib.Common.Tests.Seeds;
using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace GameLib.DAL.Tests;

public class DbContextTimerTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    private async Task<GameEntity> EnsureGameExists()
    {
        var studio = StudioSeeds.StudioEntity with
        {
            Id = Guid.NewGuid()
        };
        GameLibDbContextSut.Studios.Add(studio);
        await GameLibDbContextSut.SaveChangesAsync();

        var game = new GameEntity
        {
            Id = Guid.NewGuid(),
            StudioId = studio.Id,
            Name = GameSeeds.TestGame.Name,
            Description = GameSeeds.TestGame.Description,
            Age = GameSeeds.TestGame.Age,
            ImageUrl = GameSeeds.TestGame.ImageUrl,
            Categories = new List<CategoryEntity>(),
            Libraries = new List<LibraryEntity>(),
            Timer = new List<TimerEntity>()
        };
        GameLibDbContextSut.Games.Add(game);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        return game;
    }

    [Fact]
    public async Task AddNew_Timer_Persisted()
    {
        var game = await EnsureGameExists();

        var entity = TimerSeeds.TimerEntity with
        {
            GameId = game.Id
        };

        GameLibDbContextSut.Timer.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Timer.SingleAsync(t => t.Id == entity.Id);

        DeepAssert.Equal(entity, actual);
    }

    [Fact]
    public async Task Update_Timer_Persisted()
    {
        var game = await EnsureGameExists();

        var timerToSeed = TimerSeeds.TimerEntityUpdate with
        {
            GameId = game.Id
        };
        GameLibDbContextSut.Timer.Add(timerToSeed);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        var timerToUpdate = timerToSeed with
        {
            Time = TimeSpan.FromHours(5),
            Date = new DateTime(2025, 6, 15)
        };

        GameLibDbContextSut.Timer.Update(timerToUpdate);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Timer.SingleAsync(t => t.Id == timerToUpdate.Id);

        DeepAssert.Equal(timerToUpdate, actual);
    }

    [Fact]
    public async Task Delete_Timer_Persisted()
    {
        var game = await EnsureGameExists();

        var timerToSeed = TimerSeeds.TimerEntityDelete with
        {
            GameId = game.Id
        };
        GameLibDbContextSut.Timer.Add(timerToSeed);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        var timerToDelete = await GameLibDbContextSut.Timer
            .SingleAsync(t => t.Id == timerToSeed.Id);

        GameLibDbContextSut.Timer.Remove(timerToDelete);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var exists = await dbx.Timer.AnyAsync(t => t.Id == timerToSeed.Id);

        Assert.False(exists);
    }

    [Fact]
    public async Task GetById_Timer_Persisted()
    {
        var game = await EnsureGameExists();

        var timerToSeed = TimerSeeds.TimerEntity with
        {
            GameId = game.Id
        };
        GameLibDbContextSut.Timer.Add(timerToSeed);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Timer.SingleAsync(t => t.Id == timerToSeed.Id);

        DeepAssert.Equal(timerToSeed, actual);
    }
}