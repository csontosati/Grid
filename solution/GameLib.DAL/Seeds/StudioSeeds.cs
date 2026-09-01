using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLib.DAL.Seeds;

public static class StudioSeeds
{
    public static readonly StudioEntity DefaultStudio = new()
    {
        Id = Guid.Parse("5B4E2DF8-3D06-43CF-9ED3-2778400161A5"),
        Name = "StudioName1",
        Description = "StudioDesc1",
        Games = new List<GameEntity>()
    };

    public static DbContext SeedStudios(this DbContext dbx)
    {
        dbx.Set<StudioEntity>().Add(DefaultStudio);
        return dbx;
    }
}
