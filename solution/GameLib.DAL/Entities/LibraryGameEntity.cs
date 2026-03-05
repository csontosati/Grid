using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.DAL.Entities
{
    // Mapping table for many-to-many relationship between Library and Game
    public record LibraryGameEntity : IEntity
    {
        public Guid Id { get; set; }
        public Guid LibraryId { get; set; }
        public Guid GameId { get; set; }
        public required GameEntity Game { get; init; }
        public required LibraryEntity Library { get; init; }
    }
}