namespace GameLib.DAL.Entities
{
    public record LibraryEntity : IEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
