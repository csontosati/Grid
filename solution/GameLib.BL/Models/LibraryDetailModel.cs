
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GameLib.BL.Models;
public partial class LibraryDetailModel: ModelBase
{
    [ObservableProperty]
    public required partial string Name { get; set; }
    
    [ObservableProperty]
    public partial Guid UserId { get; set; }
    
    public ObservableCollection<GameListModel> Games { get; set; } = new();

    public static LibraryDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        UserId = Guid.Empty
    };
}

