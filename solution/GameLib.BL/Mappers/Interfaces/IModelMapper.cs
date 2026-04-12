using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.BL.Mappers.Interfaces;

public interface IModelMapper<TEntity, out TListModel, TDetailModel>
{
    TListModel MapToListModel(TEntity? entity);

    IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities)
        => entities.Select(MapToListModel);

    TDetailModel MapToDetailModel(TEntity? entity);

    TEntity MapToEntity(TDetailModel model);
}
