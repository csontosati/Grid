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
    public override ICollection<string> IncludesNavigationPathDetail =>
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
    public async Task<IList<LibraryListModel>> GetByUserAsync(Guid userId)
    {
        await using var uow = UnitOfWorkFactory.Create();

        var repo = uow.GetRepository<LibraryEntity, LibraryEntityMapper>();

        var query = repo
            .Get()
            .Where(l => l.UserId == userId);

        var entities = await query.ToListAsync();

        return ModelMapper.MapToListModel(entities).ToList();
    }
}