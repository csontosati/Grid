using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace GameLib.DAL.Factories
{
    public class DbContextSqLiteFactory : IDbContextFactory<GameLibDbContext>
    {
        private readonly DbContextOptionsBuilder<GameLibDbContext> _contextOptionsBuilder = new();
        public DbContextSqLiteFactory(string databaseName) => _contextOptionsBuilder.UseSqlite(databaseName);
        public GameLibDbContext CreateDbContext() => new(_contextOptionsBuilder.Options);
    }
}
