using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLib.DAL;

public class GameLibDbContext(DbContextOptions<GameLibDbContext> options) : DbContext(options)
{
    public DbSet<GameEntity> Games => Set<GameEntity>();
    public DbSet<StudioEntity> Studios => Set<StudioEntity>();
    public DbSet<LibraryEntity> Libraries => Set<LibraryEntity>();
    public DbSet<UserEntity> Users=> Set<UserEntity>();
    public DbSet<TimerEntity> Timer => Set<TimerEntity>();
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>()
            .HasMany(u => u.Libraries)
            .WithOne()
            .HasForeignKey(l => l.UserId);

        modelBuilder.Entity<LibraryEntity>()
            .HasMany(l => l.Games)
            .WithMany(g => g.Libraries);
        
   
        modelBuilder.Entity<GameEntity>()
            .HasMany(g => g.Categories)
            .WithMany(c => c.Games);

        modelBuilder.Entity<GameEntity>()
            .HasMany(g => g.Timer)
            .WithOne()
            .HasForeignKey(t => t.GameId);

        modelBuilder.Entity<StudioEntity>()
            .HasMany(s => s.Games)
            .WithOne(g => g.Studio)
            .HasForeignKey(g => g.StudioId);
    }

}