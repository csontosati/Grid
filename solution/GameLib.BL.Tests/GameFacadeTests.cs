using GameLib.BL.Facades;
using GameLib.BL.Models;
using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GameLib.BL.Tests;

public class GameFacadeTests : FacadeTestsBase
{
    private readonly GameFacade _facadeSut;

    public GameFacadeTests(ITestOutputHelper output) : base(output)
    {
        _facadeSut = new GameFacade(UnitOfWorkFactory, GameMapper);
    }

    private async Task<Guid> SeedDummyStudioAsync()
    {
        var studioId = Guid.NewGuid();
        await using var dbx = await DbContextFactory.CreateDbContextAsync();

        dbx.Studios.Add(new StudioEntity
        {
            Id = studioId,
            Name = "Dummy Studio",
        });

        await dbx.SaveChangesAsync();
        return studioId;
    }

    private async Task SeedGameAsync(GameEntity entity)
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        dbx.Games.Add(entity);
        await dbx.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAsync_ExistingGame_ReturnsCorrectDetailModel()
    {
        var validStudioId = await SeedDummyStudioAsync();
        var gameId = Guid.NewGuid();

        await SeedGameAsync(new GameEntity
        {
            Id = gameId,
            StudioId = validStudioId, 
            Name = "Get Test Game",
            Description = "Test",
            Age = Pegi.Eighteen,
            ImageUrl = "https://dummy.com/img.jpg"
        });
        
        var result = await _facadeSut.GetAsync(gameId);

        Assert.NotNull(result);
        Assert.Equal(gameId, result.Id);
        Assert.Equal("Get Test Game", result.Name);
    }

    [Fact]
    public async Task SaveAsync_ExistingModel_UpdatesDatabase()
    {
        var validStudioId = await SeedDummyStudioAsync(); 
        var gameId = Guid.NewGuid();

        await SeedGameAsync(new GameEntity
        {
            Id = gameId,
            StudioId = validStudioId,
            Name = "Old Name",
            Description = "Old Desc",
            Age = Pegi.Three,
            ImageUrl = "https://dummy.com/old.jpg"
        });

        var modelToUpdate = new GameDetailModel
        {
            Id = gameId,
            StudioId = validStudioId,
            Name = "Updated Name",
            Description = "Updated Desc",
            Age = Pegi.Eighteen,
            ImageUrl = "https://dummy.com/new.jpg",
            CategoryNames = new ObservableCollection<string>()
        };

        var result = await _facadeSut.SaveAsync(modelToUpdate);

        Assert.Equal("Updated Name", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_ExistingModel_RemovesFromDatabase()
    {
        var validStudioId = await SeedDummyStudioAsync(); 
        var gameId = Guid.NewGuid();

        await SeedGameAsync(new GameEntity
        {
            Id = gameId,
            StudioId = validStudioId, 
            Name = "Game to Delete",
            Description = "Delete Desc",
            Age = Pegi.Eighteen,
            ImageUrl = "https://dummy.com/del.jpg"
        });

        await _facadeSut.DeleteAsync(gameId);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var exists = await dbx.Games.AnyAsync(g => g.Id == gameId);
        Assert.False(exists);
    }

    [Fact]
    public async Task GetAsync_FilterByName_ReturnsOnlyMatchingGames()
    {
        var validStudioId = await SeedDummyStudioAsync(); 

        await SeedGameAsync(new GameEntity { Id = Guid.NewGuid(), StudioId = validStudioId, Name = "The Witcher 3", Description = "Desc", Age = Pegi.Eighteen, ImageUrl = "dummy.jpg" });
        await SeedGameAsync(new GameEntity { Id = Guid.NewGuid(), StudioId = validStudioId, Name = "Cyberpunk 2077", Description = "Desc", Age = Pegi.Eighteen, ImageUrl = "dummy.jpg" });
        await SeedGameAsync(new GameEntity { Id = Guid.NewGuid(), StudioId = validStudioId, Name = "The Witcher 2", Description = "Desc", Age = Pegi.Eighteen, ImageUrl = "dummy.jpg" });

        var filter = new GameFacade.Filter { Name = "Witcher" };

        var results = await _facadeSut.GetAsync(filter);

        var listResults = results.ToList();
        Assert.Equal(2, listResults.Count);
        Assert.Contains(listResults, g => g.Name == "The Witcher 3");
    }

    [Fact]
    public async Task GetAsync_OrderByAgeDescending_ReturnsCorrectOrder()
    {
        var validStudioId = await SeedDummyStudioAsync(); 

        await SeedGameAsync(new GameEntity { Id = Guid.NewGuid(), StudioId = validStudioId, Name = "Kids Game", Description = "Desc", Age = Pegi.Three, ImageUrl = "dummy.jpg" });
        await SeedGameAsync(new GameEntity { Id = Guid.NewGuid(), StudioId = validStudioId, Name = "Adult Game", Description = "Desc", Age = Pegi.Eighteen, ImageUrl = "dummy.jpg" });
        await SeedGameAsync(new GameEntity { Id = Guid.NewGuid(), StudioId = validStudioId, Name = "Teen Game", Description = "Desc", Age = Pegi.Twelve, ImageUrl = "dummy.jpg" });

        var filter = new GameFacade.Filter { OrderBy = "age_desc" };

        var results = await _facadeSut.GetAsync(filter);

        var listResults = results.ToList();
        Assert.Equal(3, listResults.Count);
        Assert.Equal("Adult Game", listResults[0].Name);
        Assert.Equal("Teen Game", listResults[1].Name);
        Assert.Equal("Kids Game", listResults[2].Name);
    }
}