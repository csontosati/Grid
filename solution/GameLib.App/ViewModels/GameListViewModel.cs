using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;
using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL.Entities;

namespace GameLib.App.ViewModels;
public partial class GameListViewModel(
    IFacade<GameEntity, GameListModel, GameDetailModel> gameFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
: ViewModelBase(messengerService), IRecipient<GameAddedMessage>
{
    [ObservableProperty]
    public partial IEnumerable<GameListModel> Games { get; set; } = [];

    [RelayCommand]
    protected override async Task LoadAsync()
    {
        await base.LoadAsync();
        Games = await gameFacade.GetAsync();
    }

    [RelayCommand]
    private async Task GoToAddGameAsync()
    {
        await navigationService.GoToAsync(NavigationService.GameAddPageRouteAbsolute);
    }
    public void Receive(GameAddedMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }
}