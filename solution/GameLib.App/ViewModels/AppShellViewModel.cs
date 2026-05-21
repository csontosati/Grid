using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;
using GameLib.App.ViewModels;
using GameLib.BL.Facades;
using GameLib.BL.Models;

namespace GameLib.App.ViewModels;
public partial class AppShellViewModel(
    LibraryFacade libraryFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService),
        IRecipient<UserSelectedMessage>,
        IRecipient<UserUpdatedMessage>
{
    private readonly INavigationService _navigationService = navigationService;
    private Guid _currentUserId = Guid.Empty;

    public ObservableCollection<LibraryListModel> Libraries { get; } = new();

    protected override async Task LoadAsync()
    {
        await base.LoadAsync();

        if (_currentUserId == Guid.Empty)
            return;

        var libs = await libraryFacade.GetByUserAsync(_currentUserId);

        Libraries.Clear();
        foreach (var lib in libs.OrderBy(x => x.Name))
            Libraries.Add(lib);
    }

    public void Receive(UserSelectedMessage message)
    {
        _currentUserId = message.UserId;

        // 🔥 toto je dôležité
        _ = LoadAsync();
    }

    public void Receive(UserUpdatedMessage message)
    {
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task SelectLibrary(LibraryListModel? library)
    {
        if (library is null)
            return;

        messengerService.Send(new LibrarySelectedMessage(library.Id));

        // 🔥 správny route
        await _navigationService.GoToAsync("//LibraryView");
    }
}