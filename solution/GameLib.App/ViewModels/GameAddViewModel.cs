using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;
using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using GameLib.DAL.Seeds;

namespace GameLib.App.ViewModels;

public partial class GameAddViewModel(
    IFacade<GameEntity, GameListModel, GameDetailModel> gameFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    private static readonly Guid DefaultStudioId = StudioSeeds.DefaultStudio.Id;

    [ObservableProperty]
    public partial GameDetailModel Game { get; set; } = GameDetailModel.Empty;
    public Array PegiValues => Enum.GetValues(typeof(Pegi));

    protected override Task LoadAsync()
    {
        Game = new GameDetailModel
        {
            StudioId = DefaultStudioId,
            Name = string.Empty,
            ImageUrl = string.Empty,
            Age = Pegi.Three
        };
        return Task.CompletedTask;
    }
    [RelayCommand]
    private async Task SaveAsync()
    {
        Game.StudioId = DefaultStudioId;
        Game.StudioName = "2k";
        if (string.IsNullOrWhiteSpace(Game.Name))
        {
            await Application.Current!.MainPage!.DisplayAlert("Chyba", "Název hry je povinný.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(Game.ImageUrl))
        {
            await Application.Current!.MainPage!.DisplayAlert("Chyba", "URL obrázku je povinné.", "OK");
            return;
        }

        try
        {
            Game.Id = Guid.NewGuid();

            await gameFacade.SaveAsync(Game);
            ForceDataRefreshOnNextAppearing();
            navigationService.SendBackButtonPressed();
        }
        catch (Exception ex)
        {
            var inner = ex.InnerException?.InnerException?.Message
                        ?? ex.InnerException?.Message
                        ?? ex.Message;
            await Application.Current!.MainPage!.DisplayAlert("Chyba", $"Nepodarilo sa pridať hru: {inner}", "OK");
        }
        messengerService.Send(new GameAddedMessage());
    }
    [RelayCommand]
    private async Task CancelAsync()
    {
        ForceDataRefreshOnNextAppearing();
        navigationService.SendBackButtonPressed();
    }


}