using CommunityToolkit.Mvvm.ComponentModel;

namespace GameLib.BL.Models;

public partial class TimerModel: ModelBase
{
    [ObservableProperty]
    public partial Guid GameId { get; set; }

    [ObservableProperty]
    public partial TimeSpan Time { get; set; }
    
    [ObservableProperty]
    public partial DateTime Date { get; set; }

    public static TimerModel Empty => new()
    {
        Id = Guid.Empty,
        GameId = Guid.Empty,
        Time = TimeSpan.Zero,
        Date = DateTime.Now
    };
}
