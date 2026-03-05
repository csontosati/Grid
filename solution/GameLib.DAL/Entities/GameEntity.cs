using GameLib.DAL.Enums;

namespace GameLib.DAL.Entities;

public record GameEntity : IEntity
{
    public Guid Id { get; set; }
    public Guid StudioId { get; set; }
    public required string Name { get; set; }
    public string?  Description { get; set; }
    public Pegi Age { get; set; }
    public required string ImageUrl { get; set; }
    public ICollection<LibraryEntity> Libraries { get; set; } = new List<LibraryEntity>();
    public ICollection<CategoryEntity> Categories { get; set; } = new List<CategoryEntity>();
    public ICollection<TimerEntity> Timer { get; set; } = new List<TimerEntity>();
}