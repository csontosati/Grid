using GameLib.DAL.Entities;
using GameLib.DAL.Mappers;
using GameLib.DAL;
using GameLib.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameLib.DAL.Repositories;

public class Repository<TEntity>(
    DbContext dbContext,
    IEntityMapper<TEntity> entityMapper)
    : IRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public IQueryable<TEntity> Get() => _dbSet.AsNoTracking();

    public IQueryable<TEntity> GetTracked()
        => _dbSet;

    public async ValueTask<bool> ExistsAsync(TEntity entity, CancellationToken cancellationToken = default)
        => entity.Id != Guid.Empty
           && await _dbSet.AnyAsync(e => e.Id == entity.Id, cancellationToken).ConfigureAwait(false);

    public TEntity Insert(TEntity entity)
        => _dbSet.Add(entity).Entity;

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        TEntity? existingEntity = await _dbSet
            .SingleOrDefaultAsync(e => e.Id == entity.Id, cancellationToken)
            .ConfigureAwait(false);
        if (existingEntity is null)
        {
            throw new EntityNotFoundException(typeof(TEntity), entity.Id);
        }

        entityMapper.MapToExistingEntity(existingEntity, entity);
        return existingEntity;
    }

    public async Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
    {
        TEntity? entity = await _dbSet
            .SingleOrDefaultAsync(i => i.Id == entityId, cancellationToken)
            .ConfigureAwait(false);
        if (entity is null)
        {
            throw new EntityNotFoundException(typeof(TEntity), entityId);
        }

        _dbSet.Remove(entity);
    }
}