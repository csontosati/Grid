using GameLib.BL.Models;
using GameLib.DAL;
using GameLib.DAL.Entities;

namespace GameLib.BL.Facades.Interfaces;

public interface IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : ModelBase
    where TDetailModel : ModelBase
{
    Task DeleteAsync(Guid id);

    Task<TDetailModel?> GetAsync(Guid id);
    Task<IEnumerable<TListModel>> GetAsync(object? filter = null);
    Task<TDetailModel> SaveAsync(TDetailModel model);
}