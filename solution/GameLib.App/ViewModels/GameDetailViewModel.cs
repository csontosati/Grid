using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;
using GameLib.BL.Facades;
using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using System.Collections.ObjectModel;

namespace GameLib.App.ViewModels;

public partial class GameDetailViewModel(
    IFacade<GameEntity, GameListModel, GameDetailModel> gameFacade,
    LibraryFacade libraryFacade,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService) : ViewModelBase(messengerService), IRecipient<GameSelectedMessage>, IRecipient<UserSelectedMessage>, IRecipient<UserUpdatedMessage>
{
    private Guid _gameId = Guid.Empty;
    private Guid UserId = Guid.Empty;

    public ObservableCollection<LibraryListModel> Libraries { get; } = new();

    [ObservableProperty]
    public partial GameDetailModel Game { get; set; } = GameDetailModel.Empty;

    public Array PegiValues => Enum.GetValues(typeof(Pegi));

    [ObservableProperty]
    private LibraryListModel? selectedLibrary;


    public void Receive(UserSelectedMessage message)
    {
        UserId = message.UserId;
        _ = LoadLibrariesAsync(UserId);
    }
    public void Receive(UserUpdatedMessage message)
    {
        UserId = message.UserId;
        _ = LoadLibrariesAsync(UserId);
    }
    private async Task LoadLibrariesAsync(Guid userId)
    {
        try
        {
            var libs = await libraryFacade.GetByUserAsync(userId);

            Libraries.Clear();
            foreach (var l in libs) Libraries.Add(l);
        }
        catch (Exception ex)
        {
            await alertService.DisplayAsync("Error", $"Viewing libraries unsuccessful: {ex.Message}");
        }
    }

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
            await alertService.DisplayAsync("Error", $"Loading Game detail failed: {ex.Message}");
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
            await alertService.DisplayAsync("Error", "Game Name Cannot be left empty");
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
            await alertService.DisplayAsync("Error", $"Updated Game could not be saved: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task AddToLibraryAsync()
    {
        if (Game is null)
        {
            await alertService.DisplayAsync("Error", "No game selected.");
            return;
        }

        if (SelectedLibrary is null)
        {
            await alertService.DisplayAsync("Info", "Please select a library first.");
            return;
        }

        try
        {
            await libraryFacade.AddGameAsync(SelectedLibrary.Id, Game.Id);

            messengerService.Send(new GameUpdatedMessage());
            
        }
        catch (Exception ex)
        {
            await alertService.DisplayAsync("Error", $"Could not add to library: {ex.Message}");
        }
        await alertService.DisplayAsync("Success", "Game was added to your library");
    }

    public async void Receive(GameSelectedMessage message)
    {
        _gameId = message.GameId;
        await LoadAsync();
        System.Diagnostics.Debug.WriteLine($"GameSelectedId received: GameId= {_gameId}");
    }
}
