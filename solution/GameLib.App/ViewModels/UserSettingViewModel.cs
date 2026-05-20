using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.BL.Facades;
using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL.Entities;

namespace GameLib.App.ViewModels;

public partial class UserSettingsViewModel(
    IFacade<UserEntity, UserListModel, UserDetailModel> userFacade,
    LibraryFacade libraryFacade,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<UserSelectedMessage>
{
    private Guid _currentUserId = Guid.Empty;

    [ObservableProperty]
    public partial UserDetailModel? User { get; set; }

    [ObservableProperty]
    public partial string NewLibraryName { get; set; } = string.Empty;

    public void Receive(UserSelectedMessage message)
    {
        _currentUserId = message.UserId;
        ForceDataRefreshOnNextAppearing();
        MainThread.BeginInvokeOnMainThread(async () => await LoadAsync());
    }

    [RelayCommand]
    protected override async Task LoadAsync()
    {
        if (_currentUserId == Guid.Empty) return;
        User = await userFacade.GetAsync(_currentUserId);
    }

    [RelayCommand]
    private async Task SignOutAsync()
    {
        await Shell.Current.GoToAsync(NavigationService.LandingPageRouteAbsolute);
    }

    [RelayCommand]
    private async Task SaveUserAsync()
    {
        if (User is null) return;
        await userFacade.SaveAsync(User);
    }

    [RelayCommand]
    private async Task DeleteAccountAsync()
    {
        if (_currentUserId == Guid.Empty) return;
        await userFacade.DeleteAsync(_currentUserId);
        await Shell.Current.GoToAsync(NavigationService.LandingPageRouteAbsolute);
    }

    [RelayCommand]
    private async Task AddLibraryAsync()
    {
        if (string.IsNullOrWhiteSpace(NewLibraryName) || User is null) return;
        var library = new LibraryDetailModel
        {
            Id = Guid.Empty,
            Name = NewLibraryName,
            UserId = _currentUserId
        };
        await libraryFacade.SaveAsync(library);
        NewLibraryName = string.Empty;
        ForceDataRefreshOnNextAppearing();
        await LoadAsync();
    }

    [RelayCommand]
    private async Task DeleteLibraryAsync(LibraryListModel library)
    {
        await libraryFacade.DeleteAsync(library.Id);
        User?.Libraries.Remove(library);
    }
}