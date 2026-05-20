using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
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
    [ObservableProperty]
    public partial Guid UserId { get; set; }

    [ObservableProperty]
    public partial UserDetailModel User { get; set; } = UserDetailModel.Empty;

    [ObservableProperty]
    public partial IEnumerable<LibraryListModel> Libraries { get; set; } = Array.Empty<LibraryListModel>();

    protected override async Task LoadAsync()
    {
        await base.LoadAsync();

        if (UserId == Guid.Empty) return;

        User = await userFacade.GetAsync(UserId);
        Libraries = await libraryFacade.GetByUserAsync(UserId);
    }

    public void Receive(UserSelectedMessage message)
    {
        UserId = message.UserId;
        ForceDataRefreshOnNextAppearing();
    }

    [RelayCommand]
    private async Task SignOutAsync()
    {
        await Shell.Current.GoToAsync(NavigationService.LandingPageRouteAbsolute);
    }

    [RelayCommand]
    private async Task SaveUserAsync()
    {
        await userFacade.SaveAsync(User);
    }

    [RelayCommand]
    private async Task DeleteAccountAsync()
    {
        if (UserId == Guid.Empty) return;
        await userFacade.DeleteAsync(UserId);
        await Shell.Current.GoToAsync(NavigationService.LandingPageRouteAbsolute);
    }

 



}
