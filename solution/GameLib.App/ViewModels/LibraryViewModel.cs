using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.BL.Facades;
using GameLib.BL.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using GameLib.App.Services.Interfaces;

namespace GameLib.App.ViewModels;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class LibraryViewModel(
    GameFacade gameFacade,
    LibraryFacade libraryFacade,
    IMessengerService messengerService,
    INavigationService navigationService)
    : ViewModelBase(messengerService)
{
    public Guid UserId { get; set; }

    [ObservableProperty]
    private partial IEnumerable<GameListModel> Games { get; set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        gameFacade.GetAsync()
    }

    [RelayCommand]
    private async Task GoToUserSettingsAsync()
    {
        var navigationParameters = new Dictionary<string, object?> { [nameof(UserSettingsViewModel.UserId)] = UserId };
        await navigationService.GoToAsync(NavigationService.UserSettingsPageRouteRelative, navigationParameters);
    }
}