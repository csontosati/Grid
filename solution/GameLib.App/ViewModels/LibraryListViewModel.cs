using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;
using GameLib.App.ViewModels;
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
      IRecipient<LibrarySelectedMessage>
{
    [ObservableProperty]
    public partial IEnumerable<GameListModel> Games { get; set; } = [];

    private GameFacade.Filter _currentFilter = new();

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
        "age"
    ];

    [RelayCommand]
    protected override async Task LoadAsync()
    {
        await base.LoadAsync();

        if (_currentLibraryId == Guid.Empty)
            return;

        await ReloadAsync();
    }

    private async Task ReloadAsync()
    {
        if (_currentLibraryId == Guid.Empty)
        {
            Games = [];
            return;
        }

        _currentFilter.LibraryId = _currentLibraryId;

        Games = await gameFacade.GetAsync(_currentFilter);
    }

    [RelayCommand]
    private async Task ApplyFilterAsync()
    {
        _currentFilter.Name = FilterName;
        _currentFilter.Age = FilterAge;
        _currentFilter.OrderBy = SelectedOrderBy;
        _currentFilter.LibraryId = _currentLibraryId;

        await ReloadAsync();
    }

    [RelayCommand]
    private async Task ClearFilterAsync()
    {
        FilterName = null;
        FilterAge = null;
        SelectedOrderBy = null;

        _currentFilter = new GameFacade.Filter
        {
            LibraryId = _currentLibraryId
        };

        await ReloadAsync();
    }

    public void Receive(LibrarySelectedMessage message)
    {
        _currentLibraryId = message.LibraryId;

        _currentFilter = new GameFacade.Filter
        {
            LibraryId = _currentLibraryId
        };

        _ = ReloadAsync();
    }

    [RelayCommand]
    private async Task GoToGameDetailAsync(GameListModel game)
    {
        messengerService.Send(new GameSelectedMessage(game.Id));

        await navigationService.GoToAsync(
            NavigationService.GameDetailPageAbsolute);
    }

    [RelayCommand]
    private async Task GoToAddGameAsync()
    {
        await navigationService.GoToAsync(
            NavigationService.GameAddPageRouteAbsolute);
    }
}