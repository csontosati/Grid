using GameLib.App.Services;
using GameLib.App.ViewModels;

namespace GameLib.App.Views;

public partial class LibraryView : ContentPageBase
{
    private readonly GameListViewModel _viewModel;

    public LibraryView(GameListViewModel viewModel) : base(viewModel)
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
        InitializeComponent();
        _viewModel = viewModel;
    }

    private async void OnProfilePicClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(NavigationService.UserSettingsPageAbsolute);
    }

    private async void OnGameSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() != null)
        {
            await Shell.Current.GoToAsync(nameof(GameDetailView));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}