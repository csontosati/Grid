using GameLib.BL.Facades;
using GameLib.BL.Models;
using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GameLib.BL.Tests;

public class LibraryFacadeTests : FacadeTestsBase
{
    private readonly LibraryFacade _facadeSut;

    public LibraryFacadeTests(ITestOutputHelper output) : base(output)
    {
        _facadeSut = new LibraryFacade(UnitOfWorkFactory, LibraryMapper);
    }

    private async Task<Guid> SeedDummyUserAsync()
    {
        var userId = Guid.NewGuid();
        await using var dbx = await DbContextFactory.CreateDbContextAsync();

        dbx.Users.Add(new UserEntity
        {
            Id = userId,
            UserName = "testuser",
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User"
        });

        await dbx.SaveChangesAsync();
        return userId;
    }

    private async Task<Guid> SeedDummyGameAsync()
    {
        var studioId = Guid.NewGuid();
        var gameId = Guid.NewGuid();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        dbx.Studios.Add(new StudioEntity { Id = studioId, Name = "Dummy Studio" });
        dbx.Games.Add(new GameEntity
        {
            Id = gameId,
            StudioId = studioId,
            Name = "Dummy Game",
            Age = GameLib.DAL.Enums.Pegi.Eighteen,
            ImageUrl = "dummy.jpg",
            Description = "desc"
        });

        await dbx.SaveChangesAsync();
        return gameId;
    }

    private async Task SeedLibraryAsync(LibraryEntity entity)
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        dbx.Libraries.Add(entity);
        await dbx.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAsync_ExistingLibrary_ReturnsCorrectDetailModel_WithRelationalData()
    {
        var validUserId = await SeedDummyUserAsync();
        var gameId = await SeedDummyGameAsync();
        var libraryId = Guid.NewGuid();

        await using (var dbx = await DbContextFactory.CreateDbContextAsync())
        {
            var game = await dbx.Games.SingleAsync(g => g.Id == gameId);
            var library = new LibraryEntity
            {
                Id = libraryId,
                UserId = validUserId,
                Name = "My Favorites"
            };

            library.Games.Add(game);
            dbx.Libraries.Add(library);
            await dbx.SaveChangesAsync();
        }

        var result = await _facadeSut.GetAsync(libraryId);

        Assert.NotNull(result);
        Assert.Equal(libraryId, result.Id);
        Assert.Equal("My Favorites", result.Name);
        Assert.Equal(validUserId, result.UserId);
        Assert.NotNull(result.Games);
        Assert.Single(result.Games); 
        Assert.Contains(result.Games, g => g.Id == gameId);
    }

    [Fact]
    public async Task SaveAsync_NewModel_SavesToDatabase()
    {
        var validUserId = await SeedDummyUserAsync();
        var model = new LibraryDetailModel
        {
            Id = Guid.Empty,
            UserId = validUserId,
            Name = "Backlog"
        };

        var result = await _facadeSut.SaveAsync(model);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var dbEntity = await dbx.Libraries.SingleOrDefaultAsync(l => l.Id == result.Id);
        Assert.NotNull(dbEntity);
        Assert.Equal("Backlog", dbEntity.Name);
    }

    [Fact]
    public async Task SaveAsync_ExistingModel_UpdatesDatabase()
    {
        var validUserId = await SeedDummyUserAsync();
        var libraryId = Guid.NewGuid();

        await SeedLibraryAsync(new LibraryEntity
        {
            Id = libraryId,
            UserId = validUserId,
            Name = "Old Name"
        });

        var modelToUpdate = new LibraryDetailModel
        {
            Id = libraryId,
            UserId = validUserId,
            Name = "Updated Name"
        };

        var result = await _facadeSut.SaveAsync(modelToUpdate);

        Assert.Equal("Updated Name", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_ExistingModel_RemovesFromDatabase()
    {
        var validUserId = await SeedDummyUserAsync();
        var libraryId = Guid.NewGuid();

        await SeedLibraryAsync(new LibraryEntity
        {
            Id = libraryId,
            UserId = validUserId,
            Name = "To Delete"
        });

        await _facadeSut.DeleteAsync(libraryId);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var exists = await dbx.Libraries.AnyAsync(l => l.Id == libraryId);
        Assert.False(exists);
    }

    [Fact]
    public async Task AddGameAsync_ExistingGame_AddsToLibrary()
    {
        var validUserId = await SeedDummyUserAsync();
        var gameId = await SeedDummyGameAsync();
        var libraryId = Guid.NewGuid();

        await SeedLibraryAsync(new LibraryEntity
        {
            Id = libraryId,
            UserId = validUserId,
            Name = "Empty Library"
        });

        await _facadeSut.AddGameAsync(libraryId, gameId);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var updatedLibrary = await dbx.Libraries
            .Include(l => l.Games)
            .SingleAsync(l => l.Id == libraryId);

        Assert.Single(updatedLibrary.Games);
        Assert.Contains(updatedLibrary.Games, g => g.Id == gameId); 
    }

    [Fact]
    public async Task RemoveGameAsync_ExistingGameInLibrary_RemovesFromLibrary()
    {
        var validUserId = await SeedDummyUserAsync();
        var gameId = await SeedDummyGameAsync();
        var libraryId = Guid.NewGuid();

        await using (var dbxSetup = await DbContextFactory.CreateDbContextAsync())
        {
            var game = await dbxSetup.Games.SingleAsync(g => g.Id == gameId);
            var library = new LibraryEntity
            {
                Id = libraryId,
                UserId = validUserId,
                Name = "Library With Game"
            };
            library.Games.Add(game);
            dbxSetup.Libraries.Add(library);
            await dbxSetup.SaveChangesAsync();
        }

        await _facadeSut.RemoveGameAsync(libraryId, gameId);

        await using var dbxVerify = await DbContextFactory.CreateDbContextAsync();
        var updatedLibrary = await dbxVerify.Libraries
            .Include(l => l.Games)
            .SingleAsync(l => l.Id == libraryId);

        Assert.Empty(updatedLibrary.Games); 
    }
}