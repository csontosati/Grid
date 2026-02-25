namespace GameLib.DAL
{
    public record Game
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string?  Description { get; set; }
        public required string Category { get; set; }
        public required string ImageUrl { get; set; }
    }

    public record Library
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }

    public record User
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public record Studio 
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }

    public enum GameCategory
    {
        Action,
        Adventure,
        RolePlaying,
        Strategy,
        Simulation,
        Sports,
        Racing,
        Puzzle,
        Shooter,
        Fighting,
        Platformer,
        Survival,
        Horror,
        Sandbox,
        MMO,
        BattleRoyale,
        Rhythm,
        Card,
        Board,
        Educational,
        Party,
        Indie,
        Arcade
    }
}
