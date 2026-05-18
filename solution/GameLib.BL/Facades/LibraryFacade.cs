using GameLib.BL.Mappers;
using GameLib.BL.Models;
using GameLib.DAL.Entities;
using GameLib.DAL.Mappers;
using GameLib.DAL.Repositories;
using GameLib.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace GameLib.BL.Facades;

public class LibraryFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    LibraryModelMapper modelMapper)
    : BaseFacade<
        LibraryEntity,
        LibraryListModel,
        LibraryDetailModel,
        LibraryEntityMapper>(
        unitOfWorkFactory,
        modelMapper)
{
    protected override ICollection<string> IncludesNavigationPathDetail =>
        new[]
        {
            nameof(LibraryEntity.Games)
        };

    public async Task AddGameAsync(Guid libraryId, Guid gameId)
    {
        await using var uow = UnitOfWorkFactory.Create();

        var libraryRepo = uow.GetRepository<LibraryEntity, LibraryEntityMapper>();
        var gameRepo = uow.GetRepository<GameEntity, GameEntityMapper>();

        var library = await libraryRepo
            .Get()
            .AsTracking()
            .Include(l => l.Games)
            .SingleAsync(l => l.Id == libraryId);

        var game = await gameRepo
            .Get()
            .AsTracking()
            .SingleAsync(g => g.Id == gameId);

        if (!library.Games.Any(g => g.Id == gameId))
        {
            library.Games.Add(game);
        }

        await uow.CommitAsync();
    }

    public async Task RemoveGameAsync(Guid libraryId, Guid gameId)
    {
        await using var uow = UnitOfWorkFactory.Create();

        var libraryRepo = uow.GetRepository<LibraryEntity, LibraryEntityMapper>();

        var library = await libraryRepo
            .Get()
            .AsTracking()
            .Include(l => l.Games)
            .SingleAsync(l => l.Id == libraryId);

        var game = library.Games.FirstOrDefault(g => g.Id == gameId);

        if (game is not null)
        {
            library.Games.Remove(game);
        }

        await uow.CommitAsync();
    }
    public class Filter
    {
        public string? Name { get; set; }
        public Guid? UserId { get; set; }
        public string? OrderBy { get; set; }
    }

    protected override IQueryable<LibraryEntity> ApplyFilter(
        IQueryable<LibraryEntity> query,
        object? filter)
    {
        if (filter is not Filter f)
            return query;

        if (!string.IsNullOrWhiteSpace(f.Name))
            query = query.Where(l =>
        l.Name.Contains(f.Name));

        if (f.UserId is not null)
            query = query.Where(l =>
        l.UserId == f.UserId);

        return query;
    }

    protected override IQueryable<LibraryEntity> ApplyOrder(
        IQueryable<LibraryEntity> query,
        object? filter)
    {
        if (filter is not Filter f || string.IsNullOrEmpty(f.OrderBy))
            return query;

        return f.OrderBy switch
        {
            "name" => query.OrderBy(l => l.Name),
            "name_desc" => query.OrderByDescending(l => l.Name),
            _ => query
        }
        ;
    }

}