using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameLib.App.Services;
using GameLib.BL.Facades;
using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Models;
using System.Collections.ObjectModel;

namespace GameLib.App.ViewModels;

public partial class UserListViewModel : ObservableObject
{
    private readonly UserFacade _userFacade;
    private readonly LibraryFacade _libraryFacade;
    private readonly INavigationService _navigation;
    private readonly AppState _appState;

    public ObservableCollection<UserListModel> Users { get; } = new();

    public UserListViewModel(
        UserFacade userFacade,
        LibraryFacade libraryFacade,
        INavigationService navigation,
        AppState appState)
    {
        _userFacade = userFacade;
        _libraryFacade = libraryFacade;
        _navigation = navigation;
        _appState = appState;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        Users.Clear();

        var users = await _userFacade.GetAsync();

        foreach (var user in users)
            Users.Add(user);
    }

    [RelayCommand]
    private async Task SelectUserAsync(UserListModel user)
    {
        _appState.CurrentUserId = user.Id;

        var libraries = await _libraryFacade.GetByUserAsync(user.Id);

        if (libraries.Count == 0)
        {
            await _navigation.GoToAsync(NavigationService.LibraryPageRouteAbsolute);
            return;
        }

        var firstLibrary = libraries.OrderBy(l => l.Name).First();

        _appState.CurrentLibraryId = firstLibrary.Id;

        await _navigation.GoToAsync(NavigationService.LibraryPageRouteAbsolute);
    }

    [RelayCommand]
    private async Task GoToAddUserAsync()
    {
        await _navigation.GoToAsync(NavigationService.UserAddRouteAbsolute);
    }
}
