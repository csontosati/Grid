using GameLib.Common.Tests;
using GameLib.Common.Tests.Seeds;
using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace GameLib.DAL.Tests;

public class DbContextStudioTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_Studio_Persisted()
    {
        var entity = StudioSeeds.StudioEntity;

        GameLibDbContextSut.Studios.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Studios.SingleAsync(s => s.Id == entity.Id);

        DeepAssert.Equal(entity, actual, nameof(StudioEntity.Games));
    }

    [Fact]
    public async Task Update_Studio_Persisted()
    {
        var studioToSeed = StudioSeeds.StudioEntity;
        GameLibDbContextSut.Studios.Add(studioToSeed);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        var studioToUpdate = studioToSeed with
        {
            Name = "UpdatedStudioName",
            Description = "UpdatedStudioDesc"
        };

        GameLibDbContextSut.Studios.Update(studioToUpdate);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Studios.SingleAsync(s => s.Id == studioToUpdate.Id);

        DeepAssert.Equal(studioToUpdate, actual, nameof(StudioEntity.Games));
    }

    [Fact]
    public async Task Delete_Studio_Persisted()
    {
        var studioToSeed = StudioSeeds.StudioEntityDelete;
        GameLibDbContextSut.Studios.Add(studioToSeed);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        var studioToDelete = await GameLibDbContextSut.Studios
            .SingleAsync(s => s.Id == studioToSeed.Id);

        GameLibDbContextSut.Studios.Remove(studioToDelete);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var exists = await dbx.Studios.AnyAsync(s => s.Id == studioToSeed.Id);

        Assert.False(exists);
    }

    [Fact]
    public async Task GetById_Studio_Persisted()
    {
        var studioToSeed = StudioSeeds.StudioEntityUpdate;
        GameLibDbContextSut.Studios.Add(studioToSeed);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Studios.SingleAsync(s => s.Id == studioToSeed.Id);

        DeepAssert.Equal(studioToSeed, actual, nameof(StudioEntity.Games));
    }
}
