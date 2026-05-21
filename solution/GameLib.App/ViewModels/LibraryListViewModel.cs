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

namespace GameLib.App.ViewModels;

public partial class LibraryListViewModel(
    IFacade<GameEntity, GameListModel, GameDetailModel> gameFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService),
      IRecipient<GameAddedMessage>,
      IRecipient<LibrarySelectedMessage>,
      IRecipient<GameDeletedMessage>,
      IRecipient<GameUpdatedMessage>
{
    [ObservableProperty]
    public partial IEnumerable<GameListModel> Games { get; set; } = [];

    private GameFacade.Filter? _currentFilter;
    private Guid _currentLibraryId = Guid.Empty;

    [ObservableProperty]
    public partial string? FilterName { get; set; }

    [ObservableProperty]
    public partial Pegi? FilterAge { get; set; }

    [ObservableProperty]
    public partial string? SelectedOrderBy { get; set; }

    public Array PegiValues => Enum.GetValues(typeof(Pegi));

    public IEnumerable<string> OrderByOptions { get; } =
    [
        "name",
        "age",
    ];

    [RelayCommand]
    protected override async Task LoadAsync()
    {
        await base.LoadAsync();

        if (_currentLibraryId == Guid.Empty)
            return;

        Games = await gameFacade.GetAsync(_currentFilter);
    }

    [RelayCommand]
    private async Task ApplyFilterAsync()
    {
        _currentFilter = new GameFacade.Filter
        {
            Name = FilterName,
            Age = (Pegi?)FilterAge,
            OrderBy = SelectedOrderBy
        };

        Games = await gameFacade.GetAsync(_currentFilter);
    }

    [RelayCommand]
    private async Task ClearFilterAsync()
    {
        FilterName = null;
        FilterAge = null;
        SelectedOrderBy = null;

        _currentFilter = null;

        Games = await gameFacade.GetAsync();
    }

    [RelayCommand]
    private async Task GoToAddGameAsync()
    {
        await navigationService.GoToAsync(
            NavigationService.GameAddPageRouteAbsolute);
    }

    public void Receive(GameAddedMessage message)
        => ForceDataRefreshOnNextAppearing();

    public void Receive(LibrarySelectedMessage message)
    {
        _currentLibraryId = message.LibraryId;
        ForceDataRefreshOnNextAppearing();
    }

    public void Receive(GameDeletedMessage message)
        => ForceDataRefreshOnNextAppearing();

    public void Receive(GameUpdatedMessage message)
        => ForceDataRefreshOnNextAppearing();

    [RelayCommand]
    private async Task GoToUserSettingsAsync()
    {
        await navigationService.GoToAsync(
            NavigationService.UserSettingsPageAbsolute);
    }

    [RelayCommand]
    private async Task GoToGameDetailAsync(GameListModel game)
    {
        messengerService.Send(new GameSelectedMessage(game.Id));

        await navigationService.GoToAsync(
            NavigationService.GameDetailPageAbsolute);
    }
}
