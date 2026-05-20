using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Mappers.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL;
using GameLib.DAL.Entities;
using GameLib.DAL.Mappers;
using GameLib.DAL.Repositories;
using GameLib.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Reflection;

namespace GameLib.BL.Facades;

public abstract class
    BaseFacade<TEntity, TListModel, TDetailModel, TEntityMapper>(
        IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapper<TEntity, TListModel, TDetailModel> modelMapper)
    : IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : ModelBase
    where TDetailModel : ModelBase
    where TEntityMapper : class, IEntityMapper<TEntity>
{
    protected readonly IModelMapper<TEntity, TListModel, TDetailModel> ModelMapper = modelMapper;
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory = unitOfWorkFactory;

    protected virtual ICollection<string> IncludesNavigationPathDetail => new List<string>();

    public virtual IQueryable<TEntity> ApplyFilter(
        IQueryable<TEntity> query,
        object? filter)
    {
        return query;
    }

    public virtual IQueryable<TEntity> ApplyOrder(
        IQueryable<TEntity> query,
        object? filter)
    {
        return query;
    }

    public async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        try
        {
            await uow.GetRepository<TEntity, TEntityMapper>()
                .DeleteAsync(id)
                .ConfigureAwait(false);

            await uow.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }

    public virtual async Task<TDetailModel?> GetAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<TEntity> query =
            uow.GetRepository<TEntity, TEntityMapper>().Get();

        foreach (string includePath in IncludesNavigationPathDetail)
        {
            query = query.Include(includePath);
        }

        TEntity? entity = await query
            .SingleOrDefaultAsync(e => e.Id == id)
            .ConfigureAwait(false);

        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }

    public virtual async Task<IEnumerable<TListModel>> GetAsync(object? filter = null)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<TEntity> query =
            uow.GetRepository<TEntity, TEntityMapper>().Get();

        query = ApplyFilter(query, filter);
        query = ApplyOrder(query, filter);

        List<TEntity> entities = await query
            .ToListAsync()
            .ConfigureAwait(false);

        return ModelMapper.MapToListModel(entities);
    }

    public virtual async Task<TDetailModel> SaveAsync(TDetailModel model)
    {
        GuardCollectionsAreNotSet(model);

        TEntity entity = ModelMapper.MapToEntity(model);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<TEntity> repository =
            uow.GetRepository<TEntity, TEntityMapper>();

        TDetailModel result;

        if (await repository.ExistsAsync(entity).ConfigureAwait(false))
        {
            TEntity updatedEntity = await repository
                .UpdateAsync(entity)
                .ConfigureAwait(false);

            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();

            TEntity insertedEntity = repository.Insert(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync().ConfigureAwait(false);

        return result;
    }

    private static void GuardCollectionsAreNotSet(TDetailModel model)
    {
        IEnumerable<PropertyInfo> collectionProperties = model
            .GetType()
            .GetProperties()
            .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

        foreach (PropertyInfo collectionProperty in collectionProperties)
        {
            if (collectionProperty.GetValue(model) is ICollection { Count: > 0 })
            {
                throw new InvalidOperationException(
                    "Current BL/DAL infrastructure disallows insert/update of models with collections.");
            }
        }
    }
}