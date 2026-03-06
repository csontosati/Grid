using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GameLib.DAL.Factories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GameLibDbContext>
    {
        public GameLibDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameLibDbContext>();

            optionsBuilder.UseSqlite("Data Source=gamelib_migration.db");

            return new GameLibDbContext(optionsBuilder.Options);
        }
    }
}