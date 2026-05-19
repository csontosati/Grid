using GameLib.DAL;
using Microsoft.EntityFrameworkCore;

namespace GameLib.DAL.Migrator;

public class DbMigrator(IDbContextFactory<GameLibDbContext> dbContextFactory) : IDbMigrator
{
    public void Migrate()
    {
        using var dbContext = dbContextFactory.CreateDbContext();
        dbContext.Database.Migrate();
    }
}