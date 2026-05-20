using CommunityToolkit.Mvvm.ComponentModel;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;
using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL.Entities;


namespace GameLib.App.ViewModels;

[QueryProperty(nameof(GameId), nameof(GameId))]
public partial class GameDetailViewModel(
    IFacade<GameEntity, GameListModel, GameDetailModel> gameFacade,
    INavigationService navigationService,
    IMessengerService messengerService) : ViewModelBase(messengerService)
{
    [ObservableProperty]
    private Guid _gameId;

    [ObservableProperty]
    private GameDetailModel _game = GameDetailModel.Empty;

    protected override async Task LoadAsync()
    {
        if (GameId == Guid.Empty)
            return;

        try
        {
            var loadedGame = await gameFacade.GetAsync(GameId);
            if (loadedGame is not null)
            {
                Game = loadedGame;
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Chyba", $"Nepodařilo se načíst detail hry: {ex.Message}", "OK");
        }
    }
}