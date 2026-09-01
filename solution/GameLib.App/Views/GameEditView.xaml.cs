namespace GameLib.App.Views;

public partial class GameEditView : ContentPage
{
    public GameEditView()
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
        InitializeComponent();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
        await DisplayAlert("Success", "Game added to system!", "OK");
        await Shell.Current.GoToAsync("..");
    }
}