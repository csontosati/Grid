using GameLib.BL.Models;
using GameLib.DAL.Entities;

namespace GameLib.BL.Facades.Interfaces;

public interface IGameFacade : IFacade<GameEntity, GameListModel, GameDetailModel>
{
    IQueryable<GameEntity> ApplyFilter(IQueryable<GameEntity> query, object? filter);
    IQueryable<GameEntity> ApplyOrder(IQueryable<GameEntity> query, object? filter);
}