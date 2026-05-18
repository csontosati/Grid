namespace GameLib.App.Views;

public partial class UserSelectionView : ContentPage
{
    public UserSelectionView()
    {
        InitializeComponent();
    }

    private async void OnUserSelected(object sender, EventArgs e)
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;

        await Shell.Current.GoToAsync("//LibraryView");
    }
    private async void OnAddNewClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SignUpView));
    }
}