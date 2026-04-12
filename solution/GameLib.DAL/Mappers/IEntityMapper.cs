using GameLib.DAL.Entities;
using GameLib.DAL;

namespace GameLib.DAL.Mappers;

public interface IEntityMapper<in TEntity>
    where TEntity : IEntity
{
    void MapToExistingEntity(TEntity existingEntity, TEntity newEntity);
}