using GameLib.Common.Tests;
using GameLib.Common.Tests.Seeds;
using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace GameLib.DAL.Tests;

public class DbContextUserTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_User_Persisted()
    {
        var entity = UserSeeds.UserEntity;

        GameLibDbContextSut.Users.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Users.SingleAsync(u => u.Id == entity.Id);

        DeepAssert.Equal(entity, actual, nameof(UserEntity.Libraries));
    }

    [Fact]
    public async Task Update_User_Persisted()
    {
        var userToSeed = UserSeeds.UserEntity;
        GameLibDbContextSut.Users.Add(userToSeed);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        var userToUpdate = userToSeed with
        {
            UserName = "UpdatedGamer",
            Email = "updated@test.com",
            FirstName = "Jane",
            LastName = "Doe"
        };

        GameLibDbContextSut.Users.Update(userToUpdate);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Users.SingleAsync(u => u.Id == userToUpdate.Id);

        DeepAssert.Equal(userToUpdate, actual, nameof(UserEntity.Libraries));
    }

    [Fact]
    public async Task Delete_User_Persisted()
    {
        var userToSeed = UserSeeds.UserEntityDelete;
        GameLibDbContextSut.Users.Add(userToSeed);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        var userToDelete = await GameLibDbContextSut.Users
            .SingleAsync(u => u.Id == userToSeed.Id);

        GameLibDbContextSut.Users.Remove(userToDelete);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var exists = await dbx.Users.AnyAsync(u => u.Id == userToSeed.Id);

        Assert.False(exists);
    }

    [Fact]
    public async Task GetById_User_Persisted()
    {
        var userToSeed = UserSeeds.UserEntity;
        GameLibDbContextSut.Users.Add(userToSeed);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Users.SingleAsync(u => u.Id == userToSeed.Id);

        DeepAssert.Equal(userToSeed, actual, nameof(UserEntity.Libraries));
    }

    [Fact]
    public async Task AddNew_User_With_Libraries_Persisted()
    {
        var user = UserSeeds.UserEntity;
        GameLibDbContextSut.Users.Add(user);
        await GameLibDbContextSut.SaveChangesAsync();

        var lib1 = LibrarySeeds.LibraryEntity with { UserId = user.Id };
        var lib2 = LibrarySeeds.LibraryEntity2 with { UserId = user.Id };

        GameLibDbContextSut.Libraries.AddRange(lib1, lib2);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Users
            .Include(u => u.Libraries)
            .SingleAsync(u => u.Id == user.Id);

        Assert.Equal(2, actual.Libraries.Count);
        DeepAssert.Equal(user with
        {
            Libraries = new List<LibraryEntity> { lib1, lib2 }
        }, actual, nameof(UserEntity.Libraries));
    }
}