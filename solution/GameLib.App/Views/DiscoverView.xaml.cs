using GameLib.App.ViewModels;

namespace GameLib.App.Views;

public partial class DiscoverView : ContentPageBase
{
    public DiscoverView(GameListViewModel viewModel) : base(viewModel)
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
        InitializeComponent();
    }
}