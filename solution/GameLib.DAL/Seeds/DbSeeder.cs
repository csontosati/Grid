using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GameLib.DAL.Seeds;

public class DbSeeder(IDbContextFactory<GameLibDbContext> dbContextFactory)
    : IDbSeeder
{
    public void Seed()
    {
        using GameLibDbContext dbContext = dbContextFactory.CreateDbContext();

        if (dbContext.Users.Any())
        {
            return;
        }

        if (!dbContext.Set<StudioEntity>().Any(e => e.Id == StudioSeeds.DefaultStudio.Id))
        {
            dbContext.Set<StudioEntity>().Add(StudioSeeds.DefaultStudio);
        }

        UserSeeds.SeedUsers(dbContext);

        GameSeeds.SeedGames(dbContext);

        LibrarySeeds.SeedLibraries(dbContext);

        CategorySeeds.SeedCategories(dbContext);

        TimerSeeds.SeedTimers(dbContext);

        dbContext.SaveChanges();
    }
}

