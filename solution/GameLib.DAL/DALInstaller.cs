using GameLib.DAL.Factories;
using GameLib.DAL.Mappers;
using GameLib.DAL.Migrator;
using GameLib.DAL.Seeds;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

namespace GameLib.DAL;

public static class DALInstaller
{
    public static IServiceCollection AddDALServices(this IServiceCollection services, string dbPath)
    {
        services.AddSingleton<IDbContextFactory<GameLibDbContext>>(_ =>
            new DbContextSqLiteFactory(dbPath));

        services.AddSingleton<IDbMigrator, DbMigrator>();
        services.AddSingleton<IDbSeeder, DbSeeder>();

        services.AddSingleton<UserEntityMapper>();
        services.AddSingleton<LibraryEntityMapper>();
        services.AddSingleton<GameEntityMapper>();

        return services;
    }
}