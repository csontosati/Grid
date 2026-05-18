namespace GameLib.App.Views;

public partial class GameEditView : ContentPage
{
    public GameEditView()
    {
        InitializeComponent();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Success", "Game added to system!", "OK");
        await Shell.Current.GoToAsync("..");
    }
}