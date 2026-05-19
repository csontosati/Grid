using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameLib.DAL.Seeds;

public static class CategorySeeds
{
    public static readonly CategoryEntity ActionCategory = new()
    {
        Id = Guid.Parse("8F4B97B3-4785-42DB-8ED2-46174A222160"),
        Category = GameCategory.Action
    };

    public static readonly CategoryEntity MMOCategory = new()
    {
        Id = Guid.Parse("BC6D0F99-47FE-4060-ABAF-2C1B698B05B8"),
        Category = GameCategory.MMO
    };

    public static DbContext SeedCategories(this DbContext dbx)
    {
        dbx.Set<CategoryEntity>().AddRange(ActionCategory, MMOCategory);
        return dbx;
    }
}
