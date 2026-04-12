using System;
using System.Collections.Generic;
using System.Text;
using GameLib.DAL.Entities;

namespace GameLib.DAL.Mappers
{
    public class GameEntityMapper : IEntityMapper<GameEntity>
    {
        public void MapToExistingEntity(GameEntity target, GameEntity source)
        {
            target.Name = source.Name;
            target.Description = source.Description;
            target.Age = source.Age;
            target.ImageUrl = source.ImageUrl;
            target.StudioId = source.StudioId;
        }
    }
}
