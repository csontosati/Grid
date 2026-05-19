using GameLib.DAL.Factories;
using GameLib.DAL.Mappers;
using GameLib.DAL.Options;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameLib.DAL;

public static class DALInstaller
{
    public static IServiceCollection AddDALServices(this IServiceCollection services)
    {
        services.AddSingleton<IDbContextFactory<GameLibDbContext>>(serviceProvider =>
        {
            var dalOptions = serviceProvider.GetRequiredService<IOptions<DALOptions>>();
            return new DbContextSqLiteFactory(dalOptions.Value.DatabaseFilePath);
        });
        //services.AddSingleton<IDbMigrator, DbMigrator>();
        //services.AddSingleton<IDbSeeder, DbSeeder>();

        services.AddSingleton<UserEntityMapper>();
        services.AddSingleton<LibraryEntityMapper>();
        services.AddSingleton<GameEntityMapper>();

        return services;
    }
}