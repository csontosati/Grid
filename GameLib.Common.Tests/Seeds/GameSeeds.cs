using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameLib.Common.Tests.Seeds;

public static class GameSeeds
{
    public static readonly GameEntity WitcherGame = new()
    {
        Id = Guid.Parse("758F2843-0887-4268-9A1E-A96AD820DC2A"),
        Name = "The Witcher 3",
        Description = "RPG",
        Age = Pegi.Eighteen,
        ImageUrl = "https://",
        StudioId = StudioSeeds.StudioEntity.Id
    };

    
    public static readonly GameEntity GameUpdate = WitcherGame with
    {
        Id = Guid.Parse("F85978E6-4B1A-4EBD-A58D-644AAE2FAEB1"),
        Name = "Updated Game",
        Categories = new List<CategoryEntity>(),
        Libraries = new List<LibraryEntity>(),
        Timer = new List<TimerEntity>()
    };

    public static readonly GameEntity GameDelete = WitcherGame with
    {
        Id = Guid.Parse("16851B89-31DE-438C-9A7E-EA9637B8CBF4"),
        Categories = new List<CategoryEntity>(),
        Libraries = new List<LibraryEntity>(),
        Timer = new List<TimerEntity>()
    };

    static GameSeeds()
    {
        WitcherGame.Categories.Add(CategorySeeds.MMOCategory);
        WitcherGame.Categories.Add(CategorySeeds.ActionCategory);
    }

    public static DbContext SeedGames(this DbContext dbx)
    {
      
        dbx.Set<CategoryEntity>().Attach(CategorySeeds.MMOCategory);
        dbx.Set<CategoryEntity>().Attach(CategorySeeds.ActionCategory);

        dbx.Set<GameEntity>().AddRange(WitcherGame, GameUpdate, GameDelete);
        return dbx;
    }
}