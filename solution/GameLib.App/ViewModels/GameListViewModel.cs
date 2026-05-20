using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;
using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL.Entities;

namespace GameLib.App.ViewModels;
public partial class GameListViewModel(
    IGameFacade gameFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
: ViewModelBase(messengerService)
{
    [ObservableProperty]
    public partial IEnumerable<GameListModel> Games { get; set; } = [];

    [RelayCommand]
    protected async Task LoadGamesAsync()
    {
        await base.LoadAsync();
        
        Games = await gameFacade.GetAsync();
    }
}