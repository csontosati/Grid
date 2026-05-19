using CommunityToolkit.Maui;
using GameLib.App.Services;
using GameLib.App.ViewModels;
using GameLib.App.Views;
using GameLib.BL.Facades;
using GameLib.BL.Mappers;
using GameLib.BL.Mappers.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL;
using GameLib.DAL.Entities;
using GameLib.DAL.Factories;
using GameLib.DAL.Mappers;
using GameLib.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameLib.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<AppState>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();

            string databaseName = Path.Combine(FileSystem.AppDataDirectory, "gamelib.db");
            builder.Services.AddSingleton<IDbContextFactory<GameLibDbContext>>(
                new DbContextSqLiteFactory(databaseName));
            builder.Services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            builder.Services.AddTransient<UserEntityMapper>();
            builder.Services.AddTransient<LibraryEntityMapper>();
            builder.Services.AddTransient<UserModelMapper>();
            builder.Services.AddTransient<LibraryModelMapper>();
            builder.Services.AddTransient<IModelMapper<LibraryEntity, LibraryListModel, LibraryDetailModel>>(
                sp => sp.GetRequiredService<LibraryModelMapper>());
            builder.Services.AddTransient<UserFacade>();
            builder.Services.AddTransient<LibraryFacade>();
            builder.Services.AddTransient<UserListViewModel>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<UserAddViewModel>();
            builder.Services.AddTransient<SignUpView>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}