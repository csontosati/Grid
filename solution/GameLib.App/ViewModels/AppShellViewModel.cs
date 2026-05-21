using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;
using GameLib.BL.Facades;
using GameLib.BL.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GameLib.App.ViewModels;

public partial class AppShellViewModel(
    LibraryFacade libraryFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<UserSelectedMessage>
{
    private Guid _currentUserId = Guid.Empty;

    [ObservableProperty]
    public partial IEnumerable<LibraryListModel> Libraries { get; set; } = [];

    protected override async Task LoadAsync()
    {
        if (_currentUserId == Guid.Empty) return;
        await base.LoadAsync();
        Libraries = await libraryFacade.GetByUserAsync(_currentUserId);
    }

    public async void Receive(UserSelectedMessage message)
    {
        _currentUserId = message.UserId;
        ForceDataRefreshOnNextAppearing();

        if (_currentUserId != Guid.Empty)
        {
            Libraries = await libraryFacade.GetByUserAsync(_currentUserId);
        }
    }

    [RelayCommand]
    private async Task GoToLibraryAsync(Guid libraryId)
    {
        MessengerService.Send(new LibrarySelectedMessage(libraryId));
        await navigationService.GoToAsync(NavigationService.LibraryPageRouteAbsolute);
    }

    [RelayCommand]
    private async Task GoToUserSettingsAsync()
    {
        if (_currentUserId == Guid.Empty) return;
        MessengerService.Send(new UserSelectedMessage(_currentUserId));
        await navigationService.GoToAsync(NavigationService.UserSettingsPageAbsolute);
    }
}