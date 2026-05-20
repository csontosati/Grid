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

namespace GameLib.App.ViewModels;

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class UserSettingsViewModel(
    UserFacade userFacade,
    LibraryFacade libraryFacade,
    IMessengerService messengerService,
    INavigationService navigationService)
    : ViewModelBase(messengerService)
{
    public Guid UserId { get; set; }

    [ObservableProperty]
    public required partial UserDetailModel User { get; set; }

    [ObservableProperty] 
    public partial IEnumerable<LibraryListModel> Libraries { get; set; }

    [ObservableProperty]
    public partial string NewLibraryName { get; set; } = string.Empty;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        User = await userFacade.GetAsync(UserId);
        Libraries = await libraryFacade.GetByUserAsync(UserId);
    }

    [RelayCommand]
    private async Task SignOutAsync()
    {
        await navigationService.GoToAsync(NavigationService.LandingPageRouteAbsolute);
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
        await navigationService.GoToAsync(NavigationService.LandingPageRouteAbsolute);
    }

    [RelayCommand]
    private async Task AddLibraryAsync()
    {
        if (string.IsNullOrWhiteSpace(NewLibraryName) || User is null) return;
        var library = new LibraryDetailModel
        {
            Id = Guid.Empty,
            Name = NewLibraryName,
            UserId = UserId
        };
        await libraryFacade.SaveAsync(library);
        NewLibraryName = string.Empty;
        ForceDataRefreshOnNextAppearing();
        await base.LoadDataAsync();
    }

    [RelayCommand]
    private async Task DeleteLibraryAsync(LibraryListModel library)
    {
        await libraryFacade.DeleteAsync(library.Id);
        User?.Libraries.Remove(library);
    }
}