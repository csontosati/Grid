using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;
using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using System.Collections.ObjectModel;


namespace GameLib.App.ViewModels;

public partial class GameDetailViewModel(
    IFacade<GameEntity, GameListModel, GameDetailModel> gameFacade,
    INavigationService navigationService,
    IMessengerService messengerService) : ViewModelBase(messengerService), IRecipient<GameSelectedMessage>
{

    private Guid _gameId = Guid.Empty;

    [ObservableProperty]
    public partial GameDetailModel Game { get; set; } = GameDetailModel.Empty;

    public Array PegiValues => Enum.GetValues(typeof(Pegi));

    [RelayCommand]
    protected override async Task LoadAsync()
    {
        if (_gameId == Guid.Empty)
            return;

        try
        {
            var loadedGame = await gameFacade.GetAsync(_gameId);
            if (loadedGame is not null)
            {
                Game = loadedGame;
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"Loading Game detail failed: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task DeleteGameAsync()
    {
        await gameFacade.DeleteAsync(_gameId);
        messengerService.Send(new GameDeletedMessage());
        navigationService.SendBackButtonPressed();

    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Game is null) return;

        if (string.IsNullOrWhiteSpace(Game.Name))
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "Game Name Cannot be left empty", "OK");
            return;
        }

        try
        {
            var categories = Game.CategoryNames is not null
                ? new ObservableCollection<string>(Game.CategoryNames)
                : new ObservableCollection<string>();

            var toUpdate = new GameDetailModel
            {
                Id = Game.Id,
                Name = Game.Name,
                Description = Game.Description,
                ImageUrl = Game.ImageUrl,
                StudioId = Game.StudioId,
                StudioName = Game.StudioName,
                Age = Game.Age,
                TimePlayed = Game.TimePlayed,
                CategoryNames = categories
            };

            await gameFacade.SaveAsync(toUpdate);

            var refreshed = await gameFacade.GetAsync(Game.Id);
            if (refreshed is not null)
            {
                Game = refreshed;
            }

            messengerService.Send(new GameUpdatedMessage());

            navigationService.SendBackButtonPressed();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Updated Game could not be saved: {ex.Message}", "OK");
        }
    }


    public void Receive(GameSelectedMessage message)
    {
        _gameId = message.GameId;
        System.Diagnostics.Debug.WriteLine($"GameSelectedId received: GameId= {_gameId}");
    }

}