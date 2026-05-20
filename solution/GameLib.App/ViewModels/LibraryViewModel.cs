using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.BL.Facades;
using GameLib.BL.Models;
using System.Collections.ObjectModel;

namespace GameLib.App.ViewModels;

public partial class LibraryViewModel(
    LibraryFacade libraryFacade,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<LibrarySelectedMessage>
{
    private Guid _currentLibraryId = Guid.Empty;

    [ObservableProperty]
    private LibraryDetailModel? _currentLibrary;

    public void Receive(LibrarySelectedMessage message)
    {
        _currentLibraryId = message.LibraryId;
        ForceDataRefreshOnNextAppearing();
    }

    protected override async Task LoadAsync()
    {
        if (_currentLibraryId == Guid.Empty) return;
        CurrentLibrary = await libraryFacade.GetAsync(_currentLibraryId);
    }
}