using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.BL.Facades;
using GameLib.BL.Models;
using System.Collections.ObjectModel;

namespace GameLib.App.ViewModels;

public partial class AppShellViewModel(
    LibraryFacade libraryFacade,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<UserSelectedMessage>
{
    private Guid _currentUserId = Guid.Empty;

    public ObservableCollection<LibraryListModel> Libraries { get; } = new();

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
    private async Task GoToLibraryAsync(LibraryListModel library)
    {
        MessengerService.Send(new LibrarySelectedMessage(library.Id));
        await Shell.Current.GoToAsync(NavigationService.LibraryPageRouteAbsolute);
    }
}