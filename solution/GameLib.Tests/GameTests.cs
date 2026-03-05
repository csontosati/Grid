using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Xunit;
using GameLib.Common.Tests;
using Xunit.Abstractions;

namespace GameLib.DAL.Tests;

public class DbContextGameTests(ITestOutputHelper output) : DbContextTestsBase(output)
{


    [Fact]
    public async Task AddNew_Game_Persisted()
    {
        // Arrange
        GameEntity entity = new()
        {
            Id = Guid.NewGuid(),
            Name = "Witcher3",
            Description = "RPG game",
            Age = Pegi.Eighteen,
            ImageUrl = "https://www.google.com",
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
        var categoryAction = new CategoryEntity { Id = Guid.NewGuid(), Category = GameCategory.Action };
        var categoryMmo = new CategoryEntity { Id = Guid.NewGuid(), Category = GameCategory.MMO };

        GameLibDbContextSut.Categories.AddRange(categoryAction, categoryMmo);

        GameEntity entity = new()
        {
            Id = Guid.NewGuid(),
            Name = "Witcher3",
            Description = "RPG game",
            Age = Pegi.Eighteen,
            ImageUrl = "https://www.google.com",
            Categories = new List<CategoryEntity> { categoryAction, categoryMmo }
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
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            UserName = "Tester",
            Email = "test@test.com"
        };
        GameLibDbContextSut.Users.Add(user);

        await GameLibDbContextSut.SaveChangesAsync();

        var library1 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Lib1", UserId = user.Id };
        var library2 = new LibraryEntity { Id = Guid.NewGuid(), Name = "Lib2", UserId = user.Id };

        GameEntity entity = new()
        {
            Id = Guid.NewGuid(),
            Name = "Witcher3",
            Description = "RPG game",
            Age = Pegi.Eighteen,
            ImageUrl = "https://www.google.com",
            Libraries = new List<LibraryEntity> { library1, library2 }
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
        var game = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Cyberpunk 2077",
            ImageUrl = "https://www.google.com",
        };
        GameLibDbContextSut.Games.Add(game);
        await GameLibDbContextSut.SaveChangesAsync();

        var timer1 = new TimerEntity
        {
            Id = Guid.NewGuid(),
            GameId = game.Id,
            Time = TimeSpan.FromHours(2),
            Date = new DateTime(2024, 1, 1)
        };

        var timer2 = new TimerEntity
        {
            Id = Guid.NewGuid(),
            GameId = game.Id,
            Time = TimeSpan.FromMinutes(45),
            Date = new DateTime(2024, 1, 2)
        };

        // Act
        GameLibDbContextSut.Timer.AddRange(timer1, timer2);
        await GameLibDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();

        var actualGame = await dbx.Games
            .Include(g => g.Timer)
            .SingleAsync(g => g.Id == game.Id);

       DeepAssert.Equal(game,actualGame);
    }
    [Fact]
    public async Task Update_Game_Persisted()
    {
        var game = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Witcher 2",
            Description = "Old description",
            Age = Pegi.Sixteen,
            ImageUrl = "https://www.freepik.com/free-photos-vectors/landscape"
        };
        GameLibDbContextSut.Games.Add(game);
        await GameLibDbContextSut.SaveChangesAsync();

        game.Name = "Witcher 3: Wild Hunt";
        game.Age = Pegi.Eighteen;

        GameLibDbContextSut.Games.Update(game);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games.SingleAsync(g => g.Id == game.Id);

        DeepAssert.Equal(game,actual);
    }
    [Fact]
    public async Task Delete_Game_Deletes_Associated_Timers_Persisted()
    {
        // Arrange
        var game = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Delete Me",
            ImageUrl = "https://www.google.com"
        };

        var timer = new TimerEntity
        {
            Id = Guid.NewGuid(),
            GameId = game.Id,
            Time = TimeSpan.FromMinutes(30),
            Date = DateTime.Now
        };

        // Act

        GameLibDbContextSut.Games.Add(game);
        GameLibDbContextSut.Timer.Add(timer);
        await GameLibDbContextSut.SaveChangesAsync();

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
        // Arrange
        var game = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "God of War",
            ImageUrl = "https://www.google.com",
            Age = Pegi.Eighteen
        };
        GameLibDbContextSut.Games.Add(game);
        await GameLibDbContextSut.SaveChangesAsync();

        // Act
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games.SingleAsync(g => g.Id == game.Id);

        // Assert
        DeepAssert.Equal(game, actual);
    }
    [Fact]
    public async Task GetById_IncludingCategories_Game_Persisted()
    {
        // Arrange
        var category = new CategoryEntity { Id = Guid.NewGuid(), Category = GameCategory.Action };
        var game = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Hades",
            ImageUrl = "https://www.google.com",
            Categories = new List<CategoryEntity> { category }
        };

        GameLibDbContextSut.Categories.Add(category);
        GameLibDbContextSut.Games.Add(game);
        await GameLibDbContextSut.SaveChangesAsync();

        // Act
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games
            .Include(g => g.Categories)
            .SingleAsync(g => g.Id == game.Id);

        // Assert
        DeepAssert.Equal(game, actual);
    }
}

