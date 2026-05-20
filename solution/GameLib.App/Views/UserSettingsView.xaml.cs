namespace GameLib.App.Views;

public partial class UserSettingsView : ContentPage
{
    public UserSettingsView()
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
        InitializeComponent();
    }
    private async void OnSignOutClicked(object sender, EventArgs e)
    {

        await Shell.Current.GoToAsync("//UserSelectionView");
    }
}