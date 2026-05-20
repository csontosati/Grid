namespace GameLib.App.Views;

public partial class DiscoverView : ContentPage
{
    public DiscoverView()
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
        InitializeComponent();
    }

    private async void OnProfilePicClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(UserSettingsView));
    }

    private async void OnGameSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() != null)
        {
            await Shell.Current.GoToAsync(nameof(GameDetailView));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
    private async void OnAddGameClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(GameAddView));
    }
}