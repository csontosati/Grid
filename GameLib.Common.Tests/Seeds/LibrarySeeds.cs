using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace GameLib.Common.Tests.Seeds;

public static class LibrarySeeds
{
    public static readonly LibraryEntity LibraryEntity = new()
    {
        Id = Guid.Parse("12A41D33-12BE-4A1B-864F-7AFC3F7D3B7F"),
        Name = "LibTest1",
        Description = "LibDesc1",
        UserId = UserSeeds.UserEntity.Id
    };
    public static readonly LibraryEntity LibraryEntity2 = new()
    {
        Id = Guid.Parse("554717A3-9DDE-4EF6-83E1-1063303F8CCF"),
        Name = "LibTest2",
        Description = "LibDesc2",
        UserId = UserSeeds.UserEntity.Id
    };

    public static readonly LibraryEntity LibraryUpdate = LibraryEntity with { Id = Guid.Parse("48AC27D7-3D70-478A-9128-CC58A7662D80"), Name = "Updated Library" };
    public static readonly LibraryEntity LibraryDelete = LibraryEntity with { Id = Guid.Parse("95719E3E-27D4-4FBE-B49D-758EC2EECD9D") };

    static LibrarySeeds()
    {
        LibraryEntity.Games.Add(GameSeeds.WitcherGame);
    }

    public static DbContext SeedLibraries(this DbContext dbx)
    {
        dbx.Set<LibraryEntity>().AddRange(LibraryEntity, LibraryUpdate, LibraryDelete);
        return dbx;
    }
}