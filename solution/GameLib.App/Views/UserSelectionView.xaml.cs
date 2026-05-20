using GameLib.App.ViewModels;
using GameLib.BL.Models;

namespace GameLib.App.Views;

public partial class UserSelectionView : ContentPageBase
{
    public UserSelectionView(UserListViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}