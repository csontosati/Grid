namespace GameLib.DAL.Entities;

public record LibraryEntity : IEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<GameEntity> Games { get; set; } = new List<GameEntity>();
    public Guid UserId { get; set; }
}