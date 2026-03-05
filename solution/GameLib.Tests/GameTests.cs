using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Xunit;
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
            ImageUrl = "https://www.freepik.com/free-photos-vectors/landscape",
        };

        // Act
        GameLibDbContextSut.Games.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Games.SingleAsync(g => g.Id == entity.Id);

        Assert.Equal(entity, actual);
    }

    [Fact]
    public async Task AddNew_Category_Persisted()
    {
        // Arrange
        CategoryEntity entity = new()
        {
            Id = Guid.NewGuid(),
            Category = GameCategory.Action
        };

        // Act
        GameLibDbContextSut.Categories.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Categories.SingleAsync(g => g.Id == entity.Id);

        Assert.Equal(entity, actual);
    }
}