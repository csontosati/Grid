using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;
using GameLib.BL.Facades;
using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Models;

namespace GameLib.App.ViewModels;

public partial class GameListViewModel(
    IGameFacade gameFacade,
    LibraryFacade libraryFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<GameAddedMessage>, IRecipient<UserSelectedMessage>
{
    [ObservableProperty] public partial IEnumerable<GameListModel> Games { get; set; } = [];

    private Guid _currentUserId = Guid.Empty;
    private Guid _currentLibraryId = Guid.Empty;

    protected override async Task LoadAsync()
    {
        await base.LoadAsync();

        if (_currentLibraryId == Guid.Empty)
            Games = await gameFacade.GetAsync();
        else
            Games = await gameFacade.GetByLibraryAsync(_currentLibraryId);
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

    public void Receive(UserSelectedMessage message)
    {
        _currentUserId = message.UserId;
        ForceDataRefreshOnNextAppearing();
    }

    public void Recieve(LibrarySelectedMessage message)
    {
        if (_currentLibraryId == message.LibraryId) return;
        _currentLibraryId = message.LibraryId;
        ForceDataRefreshOnNextAppearing();
    }

[RelayCommand]
    private async Task GoToUserSettingsAsync()
    {
        if (_currentUserId == Guid.Empty) return;

        await navigationService.GoToAsync(NavigationService.UserSettingsPageAbsolute);
    }
    
}