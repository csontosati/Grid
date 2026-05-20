using GameLib.DAL.Entities;
using GameLib.DAL.Enums;

namespace GameLib.BL.Facades.Interfaces;
public interface IGameFacade
{
    public class Filter
    {
        public string? Name { get; set; }
        public Pegi? Age { get; set; }
        public Guid? StudioId { get; set; }
        public string? OrderBy { get; set; }
    }


    public IQueryable<GameEntity> ApplyFilter(IQueryable<GameEntity> query, object? filter);
    public IQueryable<GameEntity> ApplyOrder(IQueryable<GameEntity> query, object? filter);
}
