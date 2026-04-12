using GameLib.BL.Facades;
using GameLib.BL.Models;
using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GameLib.BL.Tests;

public class UserFacadeTests : FacadeTestsBase
{
    private readonly UserFacade _facadeSut;

    public UserFacadeTests(ITestOutputHelper output) : base(output)
    {
        _facadeSut = new UserFacade(UnitOfWorkFactory, UserMapper);
    }

    private async Task SeedUserAsync(UserEntity entity)
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        dbx.Users.Add(entity);
        await dbx.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAsync_ExistingUser_ReturnsCorrectDetailModel()
    {
        
        var userId = Guid.NewGuid();
        await SeedUserAsync(new UserEntity
        {
            Id = userId,
            UserName = "johndoe123",
            Email = "john@doe.com",
            FirstName = "John",
            LastName = "Doe",
            Libraries = new List<LibraryEntity>()
        });

        var result = await _facadeSut.GetAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("johndoe123", result.UserName);
        Assert.Equal("john@doe.com", result.Email);
    }

    [Fact]
    public async Task SaveAsync_NewModel_SavesToDatabase()
    {
        
        var model = new UserDetailModel
        {
            Id = Guid.Empty,
            UserName = "janedoe99",
            Email = "jane@doe.com",
            FirstName = "Jane",
            LastName = "Doe",
            Libraries = new ObservableCollection<LibraryListModel>()
        };

        var result = await _facadeSut.SaveAsync(model);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var dbEntity = await dbx.Users.SingleOrDefaultAsync(u => u.Id == result.Id);

        Assert.NotNull(dbEntity);
        Assert.Equal("janedoe99", dbEntity.UserName);
        Assert.Equal("jane@doe.com", dbEntity.Email);
    }

    [Fact]
    public async Task SaveAsync_ExistingModel_UpdatesDatabase()
    {
        
        var userId = Guid.NewGuid();
        await SeedUserAsync(new UserEntity
        {
            Id = userId,
            UserName = "old_user",
            Email = "old@test.com",
            FirstName = "Old",
            LastName = "Name",
            Libraries = new List<LibraryEntity>()
        });

        var modelToUpdate = new UserDetailModel
        {
            Id = userId,
            UserName = "updated_user",
            Email = "new@test.com",
            FirstName = "Updated",
            LastName = "Name",
            Libraries = new ObservableCollection<LibraryListModel>() 
        };

        
        var result = await _facadeSut.SaveAsync(modelToUpdate);

        
        Assert.Equal("updated_user", result.UserName);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var dbEntity = await dbx.Users.SingleAsync(u => u.Id == userId);
        Assert.Equal("updated_user", dbEntity.UserName);
    }

    [Fact]
    public async Task DeleteAsync_ExistingModel_RemovesFromDatabase()
    {
        
        var userId = Guid.NewGuid();
        await SeedUserAsync(new UserEntity
        {
            Id = userId,
            UserName = "delete_me",
            Email = "del@test.com",
            FirstName = "Delete",
            LastName = "Me",
            Libraries = new List<LibraryEntity>()
        });

        
        await _facadeSut.DeleteAsync(userId);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var exists = await dbx.Users.AnyAsync(u => u.Id == userId);
        Assert.False(exists);
    }
}