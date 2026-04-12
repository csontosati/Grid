
using GameLib.DAL;
using GameLib.DAL.Mappers;
using GameLib.DAL.Repositories;

namespace GameLib.DAL.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity
        where TEntityMapper : class, IEntityMapper<TEntity>;

    Task CommitAsync(CancellationToken cancellationToken = default);
}