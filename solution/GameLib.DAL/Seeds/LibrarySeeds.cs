using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLib.DAL.Seeds;

public static class LibrarySeeds
{
    public static readonly LibraryEntity Lib1_User1 = new()
    {
        Id = Guid.Parse("12A41D33-12BE-4A1B-864F-7AFC3F7D3B7F"),
        Name = "John's Main Library",
        UserId = UserSeeds.User1.Id,
        Games = new List<GameEntity> { GameSeeds.Game1, GameSeeds.Game2, GameSeeds.Game3 }
    };

    public static readonly LibraryEntity Lib2_User1 = new()
    {
        Id = Guid.Parse("22A41D33-12BE-4A1B-864F-7AFC3F7D3B7F"),
        Name = "John's Retro Collection",
        UserId = UserSeeds.User1.Id,
        Games = new List<GameEntity> { GameSeeds.Game4 }
    };

    public static readonly LibraryEntity Lib1_User2 = new()
    {
        Id = Guid.Parse("32A41D33-12BE-4A1B-864F-7AFC3F7D3B7F"),
        Name = "Jane's Library",
        UserId = UserSeeds.User2.Id,
        Games = new List<GameEntity> { GameSeeds.Game2, GameSeeds.Game3, GameSeeds.Game5 }
    };

    public static readonly LibraryEntity Lib2_User2 = new()
    {
        Id = Guid.Parse("42A41D33-12BE-4A1B-864F-7AFC3F7D3B7F"),
        Name = "Jane's Indie Picks",
        UserId = UserSeeds.User2.Id,
        Games = new List<GameEntity> { GameSeeds.Game1, GameSeeds.Game5 }
    };

    public static readonly LibraryEntity Lib1_User3 = new()
    {
        Id = Guid.Parse("52A41D33-12BE-4A1B-864F-7AFC3F7D3B7F"),
        Name = "Alice's Library",
        UserId = UserSeeds.User3.Id,
        Games = new List<GameEntity> { GameSeeds.Game3, GameSeeds.Game4 }
    };

    public static readonly LibraryEntity Lib2_User3 = new()
    {
        Id = Guid.Parse("62A41D33-12BE-4A1B-864F-7AFC3F7D3B7F"),
        Name = "Alice's Favourites",
        UserId = UserSeeds.User3.Id,
        Games = new List<GameEntity> { GameSeeds.Game2, GameSeeds.Game5 }
    };

    public static DbContext SeedLibraries(this DbContext dbx)
    {
        dbx.Set<LibraryEntity>().AddRange(Lib1_User1, Lib2_User1, Lib1_User2, Lib2_User2, Lib1_User3, Lib2_User3);
        return dbx;
    }
}
