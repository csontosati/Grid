using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.BL.Facades;
using GameLib.BL.Models;

namespace GameLib.App.ViewModels;

public partial class UserAddViewModel : ObservableObject
{
    private readonly UserFacade _userFacade;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string surname;

    public UserAddViewModel(UserFacade userFacade, INavigationService navigation)
    {
        _userFacade = userFacade;
        _navigation = navigation;
    }

    [RelayCommand]
    private async Task CreateAccountAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Name))
        {
            await Application.Current.MainPage.DisplayAlert("Chyba", "Uživatelské jméno a Jméno jsou povinné.", "OK");
            return;
        }

        try
        {
            var newDetailModel = new UserDetailModel
            {
                UserName = Username,
                Email = Email,
                FirstName = Name,
                LastName = Surname
            };

            var savedUser = await _userFacade.SaveAsync(newDetailModel);

            var listItem = new UserListModel
            {
                Id = savedUser.Id,
                UserName = savedUser.UserName
            };

            WeakReferenceMessenger.Default.Send(new UserAddedMessage(listItem));

            await _navigation.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Chyba", $"Nepodařilo se vytvořit účet: {ex.Message}", "OK");
        }
    }
}