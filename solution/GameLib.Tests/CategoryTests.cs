using GameLib.Common.Tests;
using GameLib.Common.Tests.Seeds;
using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GameLib.DAL.Tests;

public class CategoryTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_Category_Persisted()
    {
        var entity = new CategoryEntity
        {
            Id = Guid.Parse("A1B2C3D4-E5F6-4785-8ED2-46174A222160"),
            Category = GameCategory.MMO
        };

        GameLibDbContextSut.Categories.Add(entity);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Categories.SingleAsync(c => c.Id == entity.Id);

        DeepAssert.Equal(entity, actual);
    }

    [Fact]
    public async Task GetById_Category_Persisted()
    {
        GameLibDbContextSut.Categories.Add(CategorySeeds.ActionCategory);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Categories.SingleOrDefaultAsync(c => c.Id == CategorySeeds.ActionCategory.Id);

        DeepAssert.Equal(CategorySeeds.ActionCategory, actual);
    }

    [Fact]
    public async Task Update_Category_Persisted()
    {
        GameLibDbContextSut.Categories.Add(CategorySeeds.MMOCategory);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        var categoryToUpdate = await GameLibDbContextSut.Categories.SingleAsync(c => c.Id == CategorySeeds.MMOCategory.Id);
        categoryToUpdate.Category = GameCategory.Strategy;
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actual = await dbx.Categories.SingleAsync(c => c.Id == CategorySeeds.MMOCategory.Id);

        DeepAssert.Equal(categoryToUpdate, actual);
    }

    [Fact]
    public async Task Delete_Category_Persisted()
    {
        GameLibDbContextSut.Categories.Add(CategorySeeds.ActionCategory);
        await GameLibDbContextSut.SaveChangesAsync();
        GameLibDbContextSut.ChangeTracker.Clear();

        var categoryToDelete = await GameLibDbContextSut.Categories.SingleAsync(c => c.Id == CategorySeeds.ActionCategory.Id);
        GameLibDbContextSut.Categories.Remove(categoryToDelete);
        await GameLibDbContextSut.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var exists = await dbx.Categories.AnyAsync(c => c.Id == CategorySeeds.ActionCategory.Id);

        Assert.False(exists);
    }
}