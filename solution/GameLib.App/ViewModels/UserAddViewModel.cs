using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameLib.App.Services;
using GameLib.App.Services.Interfaces;
using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL.Entities;

namespace GameLib.App.ViewModels;

public partial class UserAddViewModel(
    IFacade<UserEntity, UserListModel, UserDetailModel> userFacade,
    INavigationService navigationService,
    IMessengerService messengerService) : ViewModelBase(messengerService)
{
    [ObservableProperty]
    public partial UserDetailModel User { get; set; } = UserDetailModel.Empty;

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(User.UserName))
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "Username is a required field.", "OK");
            return;
        }

        try
        {
            User.Id = Guid.NewGuid();
            await userFacade.SaveAsync(User);
            navigationService.SendBackButtonPressed();
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"Account Creation Unsuccessful: {ex.Message}", "OK");
        }
    }
}