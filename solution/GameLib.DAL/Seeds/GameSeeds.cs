using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameLib.DAL.Seeds;

public static class GameSeeds
{
    public static readonly GameEntity Game1 = new()
    {
        Id = Guid.Parse("758F2843-0887-4268-9A1E-A96AD820DC2A"),
        Name = "Great Game",
        Description = "RPG",
        Age = Pegi.Eighteen,
        ImageUrl = "https://example.com/game1.png",
        StudioId = StudioSeeds.DefaultStudio.Id,
        Categories = new List<CategoryEntity>(),
        Libraries = new List<LibraryEntity>(),
        Timer = new List<TimerEntity>()
    };

    public static readonly GameEntity Game2 = new()
    {
        Id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
        Name = "Puzzle Master",
        Description = "Puzzle",
        Age = Pegi.Three,
        ImageUrl = "https://example.com/game2.png",
        StudioId = StudioSeeds.DefaultStudio.Id,
        Categories = new List<CategoryEntity>(),
        Libraries = new List<LibraryEntity>(),
        Timer = new List<TimerEntity>()
    };

    public static readonly GameEntity Game3 = new()
    {
        Id = Guid.Parse("22222222-3333-4444-5555-666666666666"),
        Name = "Shooter Pro",
        Description = "FPS",
        Age = Pegi.Sixteen,
        ImageUrl = "https://example.com/game3.png",
        StudioId = StudioSeeds.DefaultStudio.Id,
        Categories = new List<CategoryEntity>(),
        Libraries = new List<LibraryEntity>(),
        Timer = new List<TimerEntity>()
    };

    public static readonly GameEntity Game4 = new()
    {
        Id = Guid.Parse("33333333-4444-5555-6666-777777777777"),
        Name = "Adventure Quest",
        Description = "Adventure",
        Age = Pegi.Seven,
        ImageUrl = "https://example.com/game4.png",
        StudioId = StudioSeeds.DefaultStudio.Id,
        Categories = new List<CategoryEntity>(),
        Libraries = new List<LibraryEntity>(),
        Timer = new List<TimerEntity>()
    };

    public static readonly GameEntity Game5 = new()
    {
        Id = Guid.Parse("44444444-5555-6666-7777-888888888888"),
        Name = "Strategy King",
        Description = "Strategy",
        Age = Pegi.Twelve,
        ImageUrl = "https://example.com/game5.png",
        StudioId = StudioSeeds.DefaultStudio.Id,
        Categories = new List<CategoryEntity>(),
        Libraries = new List<LibraryEntity>(),
        Timer = new List<TimerEntity>()
    };

    public static DbContext SeedGames(this DbContext dbx)
    {
        dbx.Set<GameEntity>().AddRange(Game1, Game2, Game3, Game4, Game5);
        return dbx;
    }
}
