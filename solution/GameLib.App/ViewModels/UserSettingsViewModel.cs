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

    private bool _isDeleted = false;

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
        _isDeleted = false;
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
        if (UserId == Guid.Empty) return;

        if (string.IsNullOrWhiteSpace(User.UserName))
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "Username is a required field.", "OK");
            return;
        }

        try
        {
            await userFacade.SaveAsync(User);
            MessengerService.Send(new UserUpdatedMessage(UserId));
            await Application.Current!.MainPage!.DisplayAlert("Success", "Profile saved successfully.", "OK");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"Failed to save profile: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task DeleteAccountAsync()
    {
        if (UserId == Guid.Empty || _isDeleted) return;

        bool confirmed = await Application.Current!.MainPage!.DisplayAlert(
            "Delete Account",
            "Are you sure you want to delete this account? This action cannot be undone.",
            "Delete", "Cancel");

        if (!confirmed) return;

        _isDeleted = true;
        var deletedId = UserId;

        await userFacade.DeleteAsync(deletedId);

        MessengerService.Send(new UserDeletedMessage(deletedId));

        UserId = Guid.Empty;
        User = UserDetailModel.Empty;

        await Shell.Current.GoToAsync(NavigationService.LandingPageRouteAbsolute);
    }
}