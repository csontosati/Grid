using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLib.Common.Tests.Seeds;

public static class StudioSeeds
{
    public static StudioEntity EmptyStudio => new()
    {
        Id = default,
        Name = default!
    };

    public static StudioEntity StudioEntity => new()
    {
        Id = Guid.Parse("5B4E2DF8-3D06-43CF-9ED3-2778400161A5"),
        Name = "StudioName1",
        Description = "StudioDesc1",
        Games = new List<GameEntity>()
    };

    public static StudioEntity StudioEntityUpdate => new()
    {
        Id = Guid.Parse("A5AD7F8A-549C-4971-BAD4-9607DA407C95"),
        Name = "StudioName2",
        Description = "StudioDesc2",
        Games = new List<GameEntity>()
    };

    public static StudioEntity StudioEntityDelete => new()
    {
        Id = Guid.Parse("AC85B328-73DF-4A72-BF27-C6610BDC745B"),
        Name = "StudioName1",
        Description = "StudioDesc1",
        Games = new List<GameEntity>()
    };

    public static DbContext SeedStudios(this DbContext dbx)
    {
        dbx.Set<StudioEntity>().AddRange(
            StudioEntity,
            StudioEntityUpdate,
            StudioEntityDelete
        );
        return dbx;
    }
}