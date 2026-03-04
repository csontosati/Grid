using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace GameLib.DAL;

public class GameLibDbContext(DbContextOptions<GameLibDbContext> options) : DbContext(options)
{
    public DbSet<GameEntity> Games => Set<GameEntity>();

    public DbSet<StudioEntity> Studios => Set<StudioEntity>();

    public DbSet<LibraryEntity> Libraries => Set<LibraryEntity>();

    public DbSet<UserEntity> Users=> Set<UserEntity>();

    public DbSet<GameCategoryEntity> GameCategories => Set<GameCategoryEntity>();

    public DbSet<TimerEntity> Timer => Set<TimerEntity>();

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {       
    //        optionsBuilder.
    //    }
    //}

}