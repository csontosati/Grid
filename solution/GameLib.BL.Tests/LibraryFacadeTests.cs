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

    private async Task SeedLibraryAsync(LibraryEntity entity)
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        dbx.Libraries.Add(entity);
        await dbx.SaveChangesAsync();
    }

    // --- STANDARD CRUD TESTS ---

    [Fact]
    public async Task GetAsync_ExistingLibrary_ReturnsCorrectDetailModel()
    {
        var validUserId = await SeedDummyUserAsync();
        var libraryId = Guid.NewGuid();

        await SeedLibraryAsync(new LibraryEntity
        {
            Id = libraryId,
            UserId = validUserId,
            Name = "My Favorites"
        });

        var result = await _facadeSut.GetAsync(libraryId);

        Assert.NotNull(result);
        Assert.Equal(libraryId, result.Id);
        Assert.Equal("My Favorites", result.Name);
        Assert.Equal(validUserId, result.UserId);
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
}