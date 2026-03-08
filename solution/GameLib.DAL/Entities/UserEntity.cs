namespace GameLib.DAL.Entities;

public record UserEntity : IEntity
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public ICollection<LibraryEntity> Libraries { get; set; } = new List<LibraryEntity>();
}