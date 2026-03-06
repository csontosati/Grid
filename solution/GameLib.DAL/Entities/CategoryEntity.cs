using System;
using System.Collections.Generic;
using System.Text;
using GameLib.DAL.Enums;

namespace GameLib.DAL.Entities;

public record CategoryEntity : IEntity
{
    public Guid Id { get; set; }
    public GameCategory Category { get; set; }
    public ICollection<GameEntity> Games { get; set; } = new List<GameEntity>();
}