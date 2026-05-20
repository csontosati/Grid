using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;
using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL.Entities;

namespace GameLib.App.ViewModels;

public partial class UserListViewModel(
    IFacade<UserEntity, UserListModel, UserDetailModel> userFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    [ObservableProperty]
    public partial IEnumerable<UserListModel> Users { get; set; } = [];

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Users = await userFacade.GetAsync();
    }

    [RelayCommand]
    private async Task GoToAddUserAsync()
    {
        await navigationService.GoToAsync(NavigationService.UserAddPageRouteRelative);
    }

    [RelayCommand]
    private async Task GoToLibraryAsync(Guid userId)
    {
        var navigationParameters = new Dictionary<string, object?> { [nameof(LibraryViewModel.UserId)] = userId };
        await navigationService.GoToAsync(NavigationService.LibraryPageRouteRelative, navigationParameters);
    }
}