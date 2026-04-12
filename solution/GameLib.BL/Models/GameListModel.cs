using CommunityToolkit.Mvvm.ComponentModel;

namespace GameLib.BL.Models;

public partial class GameListModel : ModelBase
{
    [ObservableProperty]
    public required partial string Name { get; set; }

    [ObservableProperty]
    public partial string? ImageUrl { get; set; }

    public static GameListModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        ImageUrl = string.Empty,
    };
}