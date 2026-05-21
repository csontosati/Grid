using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Mappers;
using GameLib.BL.Models;
using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using GameLib.DAL.Mappers;
using GameLib.DAL.UnitOfWork;
using System.Linq;

namespace GameLib.BL.Facades;

public class GameFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    GameModelMapper modelMapper)
    : BaseFacade<
        GameEntity,
        GameListModel,
        GameDetailModel,
        GameEntityMapper>(
            unitOfWorkFactory,
            modelMapper), IGameFacade
{
    public class Filter
    {
        public string? Name { get; set; }
        public Pegi? Age { get; set; }
        public Guid? StudioId { get; set; }
        public Guid? LibraryId { get; set; }
        public string? OrderBy { get; set; }
    }

    public override ICollection<string> IncludesNavigationPathDetail =>
        new[]
        {
            nameof(GameEntity.Studio),
            nameof(GameEntity.Categories),
            nameof(GameEntity.Timer),
            nameof(GameEntity.Libraries)
        };

    public override IQueryable<GameEntity> ApplyFilter(
        IQueryable<GameEntity> query,
        object? filter)
    {
        if (filter is not Filter f)
            return query;

        if (!string.IsNullOrWhiteSpace(f.Name))
        {
            query = query.Where(g => g.Name.Contains(f.Name));
        }

        if (f.Age is not null)
        {
            query = query.Where(g => g.Age == f.Age);
        }

        if (f.StudioId is not null)
        {
            query = query.Where(g => g.StudioId == f.StudioId);
        }

        if (f.LibraryId is not null)
        {
            query = query.Where(g =>
                g.Libraries.Any(l => l.Id == f.LibraryId));
        }

        return query;
    }
    public override IQueryable<GameEntity> ApplyOrder(
        IQueryable<GameEntity> query,
        object? filter)
    {
        if (filter is not Filter f || string.IsNullOrEmpty(f.OrderBy))
            return query;

        return f.OrderBy switch
        {
            "name" => query.OrderBy(g => g.Name),

            "age" => query.OrderBy(g => g.Age),
            _ => query
        };
    }
}