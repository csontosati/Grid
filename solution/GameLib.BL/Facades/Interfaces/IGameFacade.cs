using GameLib.BL.Models;
using GameLib.DAL.Entities;

namespace GameLib.BL.Facades.Interfaces;

public interface IGameFacade : IFacade<GameEntity, GameListModel, GameDetailModel>
{
    protected ICollection<string> IncludesNavigationPathDetails();
    protected IQueryable<GameEntity> ApplyFilter(IQueryable<GameEntity> query, object? filter);
    protected IQueryable<GameEntity> ApplyOrder(IQueryable<GameEntity> query, object? filter);
}