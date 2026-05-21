using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.App.ViewModels;
using GameLib.BL.Models;

namespace GameLib.App;

public partial class AppShell : Shell
{
    public AppShellViewModel ViewModel { get; }
    private readonly IMessengerService _messengerService;

    public AppShell(AppShellViewModel viewModel, IMessengerService messengerService)
    {
        InitializeComponent();

        BindingContext = ViewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.OnAppearingAsync();
    }
}