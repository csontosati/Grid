using GameLib.App.ViewModels;

namespace GameLib.App.Views;

public partial class UserSelectionView : ContentPage
{
    public UserSelectionView()
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
        InitializeComponent();
        BindingContext = IPlatformApplication.Current!.Services.GetService<UserListViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UserListViewModel vm)
            await vm.LoadCommand.ExecuteAsync(null);
    }
}