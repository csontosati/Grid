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
        ImageUrl = "https://img.magnific.com/free-vector/game-boy-vector-text-effect_17005-2497.jpg?semt=ais_hybrid&w=740&q=80",
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
        ImageUrl = "https://i.pinimg.com/736x/7d/84/66/7d8466b226631b3430f1b4d0a1f232d6.jpg",
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
        ImageUrl = "https://img.craftpix.net/2020/03/Game-Title-Pack-1.webp",
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
        ImageUrl = "https://img.craftpix.net/2020/03/Game-Title-Pack-2.jpg",
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
        ImageUrl = "https://cdn.dribbble.com/userupload/28557901/file/original-2fb8f71e4b00be976d0e85318da1ae6f.png?resize=400x0",
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
