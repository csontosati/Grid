using GameLib.BL.Mappers.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL.Entities;
using GameLib.DAL.Mappers;
using GameLib.DAL.UnitOfWork;

namespace GameLib.BL.Facades.Interfaces;

public interface IGameFacade : IFacade<GameEntity, GameListModel, GameDetailModel>
{
    IQueryable<GameEntity> ApplyFilter(IQueryable<GameEntity> query, object? filter);
    IQueryable<GameEntity> ApplyOrder(IQueryable<GameEntity> query, object? filter);
    Task<IEnumerable<GameListModel>> GetByLibraryAsync(Guid libraryId);
}