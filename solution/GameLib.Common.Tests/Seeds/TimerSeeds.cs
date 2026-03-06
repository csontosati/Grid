using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLib.Common.Tests.Seeds;

public static class TimerSeeds
{
    public static TimerEntity TimerEntity => new()
    {
        Id = Guid.Parse("A1B2C3D4-0001-4000-8000-000000000001"),
        GameId = GameSeeds.TestGame.Id,
        Time = TimeSpan.FromHours(2),
        Date = new DateTime(2024, 1, 1)
    };

    public static TimerEntity TimerEntityUpdate => new()
    {
        Id = Guid.Parse("A1B2C3D4-0002-4000-8000-000000000002"),
        GameId = GameSeeds.TestGame.Id,
        Time = TimeSpan.FromMinutes(45),
        Date = new DateTime(2024, 2, 1)
    };

    public static TimerEntity TimerEntityDelete => new()
    {
        Id = Guid.Parse("A1B2C3D4-0003-4000-8000-000000000003"),
        GameId = GameSeeds.TestGame.Id,
        Time = TimeSpan.FromHours(1),
        Date = new DateTime(2024, 3, 1)
    };

    public static DbContext SeedTimers(this DbContext dbx)
    {
        dbx.Set<TimerEntity>().AddRange(
            TimerEntity,
            TimerEntityUpdate,
            TimerEntityDelete
        );
        return dbx;
    }
}