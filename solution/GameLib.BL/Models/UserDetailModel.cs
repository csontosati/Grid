using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GameLib.BL.Models;

public partial class UserDetailModel: ModelBase
{
    [ObservableProperty]
    public required partial string UserName { get; set; }

    [ObservableProperty]
    public required partial string Email { get; set; }

    [ObservableProperty]
    public partial string? FirstName { get; set; }

    [ObservableProperty]
    public partial string? LastName { get; set; }

    public ObservableCollection<LibraryListModel> Libraries { get; set; } = new();

    public static UserDetailModel Empty => new()
    {
        Id = Guid.Empty,
        UserName = string.Empty,
        Email = string.Empty,
        FirstName = string.Empty,
        LastName = string.Empty
    };
}