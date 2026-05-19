namespace GameLib.App.Views;

public partial class UserSettingsView : ContentPage
{
    public UserSettingsView()
    {
        InitializeComponent();
    }
    private async void OnSignOutClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;

        await Shell.Current.GoToAsync("//UserSelectionView");
    }
}