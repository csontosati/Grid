using GameLib.App.Services;
using GameLib.App.ViewModels;

namespace GameLib.App.Views;

public partial class LibraryView : ContentPageBase
{
    public LibraryView(LibraryViewModel viewModel) : base(viewModel)
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
        InitializeComponent();
    }

    private async void OnProfilePicClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(NavigationService.UserSettingsPageRoute);
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