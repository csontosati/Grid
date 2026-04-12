using CommunityToolkit.Mvvm.ComponentModel;

namespace GameLib.BL.Models;
public partial class UserListModel: ModelBase
{
    [ObservableProperty]
    public required partial string UserName { get; set; }


    public static UserListModel Empty
        => new()
        {
            Id = Guid.Empty,
            UserName = string.Empty,
        };
}