using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLib.DAL.Seeds;

public static class TimerSeeds
{
    public static readonly TimerEntity DefaultTimer = new()
    {
        Id = Guid.Parse("A1B2C3D4-0001-4000-8000-000000000001"),
        GameId = GameSeeds.Game1.Id,
        Time = TimeSpan.FromHours(2),
        Date = new DateTime(2024, 1, 1)
    };

    public static DbContext SeedTimers(this DbContext dbx)
    {
        dbx.Set<TimerEntity>().Add(DefaultTimer);
        return dbx;
    }
}
