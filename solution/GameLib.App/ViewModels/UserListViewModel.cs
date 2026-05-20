using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
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
    : ViewModelBase(messengerService), IRecipient<UserDeletedMessage>, IRecipient<UserAddedMessage>, IRecipient<UserUpdatedMessage>
{
    [ObservableProperty]
    public partial IEnumerable<UserListModel> Users { get; set; } = [];

    [RelayCommand]
    protected override async Task LoadAsync()
    {
        await base.LoadAsync();
        Users = await userFacade.GetAsync();
    }

    public void Receive(UserDeletedMessage message)
    {
        ForceDataRefreshOnNextAppearing();
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            Users = await userFacade.GetAsync();
        });
    }

    public void Receive(UserAddedMessage message)
    {
        ForceDataRefreshOnNextAppearing();
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            Users = await userFacade.GetAsync();
        });
    }

    public void Receive(UserUpdatedMessage message)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            Users = await userFacade.GetAsync();
        });
    }

    [RelayCommand]
    private async Task GoToAddUserAsync()
    {
        await navigationService.GoToAsync(NavigationService.UserAddPageRouteRelative);
    }

    [RelayCommand]
    private async Task GoToLibraryAsync(UserListModel user)
    {
        MessengerService.Send(new UserSelectedMessage(user.Id));
        await navigationService.GoToAsync(NavigationService.LibraryPageRouteAbsolute);
    }
}