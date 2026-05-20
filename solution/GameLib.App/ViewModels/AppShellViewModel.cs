using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;
using GameLib.BL.Facades;
using GameLib.BL.Models;
using System.Collections.ObjectModel;

namespace GameLib.App.ViewModels;

public partial class AppShellViewModel(
    LibraryFacade libraryFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<UserSelectedMessage>
{
    private readonly INavigationService _navigationService = navigationService;
    private Guid _currentUserId = Guid.Empty;

    public ObservableCollection<LibraryListModel> Libraries { get; } = new();

    [RelayCommand]
    private async Task SelectLibrary(LibraryListModel? library)
    {
        if (library == null) return;

        await _navigationService.GoToDataAsync("//LibraryView", new Dictionary<string, object?> { { "Id", library.Id } });
    }

    protected override async Task LoadAsync()
    {
        if (_currentUserId == Guid.Empty) return;

        Libraries.Clear();
        var libs = await libraryFacade.GetByUserAsync(_currentUserId);
        foreach (var lib in libs.OrderBy(l => l.Name))
            Libraries.Add(lib);
    }

    public void Receive(UserSelectedMessage message)
    {
        _currentUserId = message.UserId;
        ForceDataRefreshOnNextAppearing();
    }
    [RelayCommand]
    private async Task GoToUserSettingsAsync()
    {
        //Debug.WriteLine($"AppShellViewModel.GoToUserSettingsAsync called. CurrentUserId = {_currentUserId}");

        if (_currentUserId == Guid.Empty)
        {
            //Debug.WriteLine("No user selected, aborting navigation to UserSettings.");
            await Application.Current.MainPage.DisplayAlert("Info", "No user selected.", "OK");
            return;
        }

        // notify other viewmodels
        messengerService.Send(new UserSelectedMessage(_currentUserId));

        // navigate to UserSettings page with parameter
        await _navigationService.GoToDataAsync(
            NavigationService.UserSettingsPageAbsolute,
            new Dictionary<string, object?> { { "UserId", _currentUserId } });
    }


}