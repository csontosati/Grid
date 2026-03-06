using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLib.Common.Tests.Seeds;

public static class LibrarySeeds
{
    public static readonly LibraryEntity LibraryEntity = new()
    {
        Id = Guid.Parse("12A41D33-12BE-4A1B-864F-7AFC3F7D3B7F"),
        Name = "LibTest1",
        Description = "LibDesc1",
        UserId = UserSeeds.UserEntity.Id,
        Games = new List<GameEntity>()
    };

    public static readonly LibraryEntity LibraryEntity2 = new()
    {
        Id = Guid.Parse("554717A3-9DDE-4EF6-83E1-1063303F8CCF"),
        Name = "LibTest2",
        Description = "LibDesc2",
        UserId = UserSeeds.UserEntity.Id,
        Games = new List<GameEntity>()
    };

    public static readonly LibraryEntity LibraryUpdate = LibraryEntity with
    {
        Id = Guid.Parse("48AC27D7-3D70-478A-9128-CC58A7662D80"),
        Name = "Updated Library",
        Games = new List<GameEntity>()
    };

    public static readonly LibraryEntity LibraryDelete = LibraryEntity with
    {
        Id = Guid.Parse("95719E3E-27D4-4FBE-B49D-758EC2EECD9D"),
        Games = new List<GameEntity>()
    };

    static LibrarySeeds()
    {
        LibraryEntity.Games.Add(GameSeeds.WitcherGame);
    }

    public static DbContext SeedLibraries(this DbContext dbx)
    {
        dbx.Set<GameEntity>().Attach(GameSeeds.WitcherGame);

        var entities = new[] { LibraryEntity, LibraryEntity2, LibraryUpdate, LibraryDelete };

        foreach (var entity in entities)
        {
            if (dbx.Set<LibraryEntity>().Local.All(e => e.Id != entity.Id))
            {
                dbx.Set<LibraryEntity>().Add(entity);
            }
        }

        return dbx;
    }
}