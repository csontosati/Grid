namespace GameLib.DAL.Entities;

public record TimerEntity : IEntity
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public TimeSpan Time { get; set; }
    public DateTime Date { get; set; }
}