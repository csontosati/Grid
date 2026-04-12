using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GameLib.DAL.Enums;

namespace GameLib.BL.Models;

public partial class GameDetailModel: ModelBase
{
    [ObservableProperty]
    public required partial Guid StudioId { get; set; }

    [ObservableProperty]
    public partial string? StudioName { get; set; }

    [ObservableProperty]
    public required partial string Name { get; set; }

    [ObservableProperty]
    public partial string? Description { get; set; }

    [ObservableProperty]
    public required partial Pegi Age { get; set; }

    [ObservableProperty]
    public required partial string ImageUrl { get; set; }

    [ObservableProperty]
    public partial TimeSpan TimePlayed { get; set; }

    public ObservableCollection<string> CategoryNames { get; set; } = new();

    public static GameDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        ImageUrl = string.Empty,
        StudioId = Guid.Empty,
        Age = Pegi.None,
    };
}