using GameLib.DAL.Enums;

namespace GameLib.DAL.Entities;

public record GameEntity : IEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string?  Description { get; set; }
    public Pegi Age { get; set; }
    public required string ImageUrl { get; set; }
}