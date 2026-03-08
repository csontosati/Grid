using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameLib.Common.Tests.Seeds;

public static class GameSeeds
{
    public static readonly GameEntity TestGame = new()
    {
        Id = Guid.Parse("758F2843-0887-4268-9A1E-A96AD820DC2A"),
        Name = "Great Game",
        Description = "RPG",
        Age = Pegi.Eighteen,
        ImageUrl = "https://",
        StudioId = StudioSeeds.StudioEntity.Id,
        Categories = new List<CategoryEntity>(),
        Libraries = new List<LibraryEntity>(),
        Timer = new List<TimerEntity>()
    };

    public static readonly GameEntity GameWithCategories = TestGame with
    {
        Id = Guid.Parse("9682F1D0-B5E7-42D9-9339-DAD1F6921431"),
        Name = "Game with Categories"
    };

    public static readonly GameEntity GameWithLibraries = TestGame with
    {
        Id = Guid.Parse("944B9394-EECC-4807-BC67-06F59FC32EF5"),
        Name = "Game in Library"
    };

    public static readonly GameEntity GameUpdate = TestGame with
    {
        Id = Guid.Parse("F85978E6-4B1A-4EBD-A58D-644AAE2FAEB1"),
        Name = "Updated Game"
    };

    public static readonly GameEntity GameDelete = TestGame with
    {
        Id = Guid.Parse("16851B89-31DE-438C-9A7E-EA9637B8CBF4"),
        Name = "Game to Delete"
    };

    static GameSeeds()
    {
        TestGame.Categories.Add(CategorySeeds.MMOCategory);
        TestGame.Categories.Add(CategorySeeds.ActionCategory);
    }

    public static DbContext SeedGames(this DbContext dbx)
    {
        dbx.Set<CategoryEntity>().Attach(CategorySeeds.MMOCategory);
        dbx.Set<CategoryEntity>().Attach(CategorySeeds.ActionCategory);

        dbx.Set<GameEntity>().AddRange(
            TestGame,
            GameWithCategories,
            GameWithLibraries,
            GameUpdate,
            GameDelete);

        return dbx;
    }
}