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

[QueryProperty(nameof(UserId), nameof(UserId))]
public partial class UserSettingsViewModel(
    IFacade<UserEntity, UserListModel, UserDetailModel> userFacade,
    LibraryFacade libraryFacade,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<UserSelectedMessage>, IQueryAttributable
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

    // Called when QueryProperty sets UserId via Shell navigation
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null) return;

        if (query.TryGetValue("UserId", out var idObj))
        {
            if (idObj is Guid guid)
            {
                UserId = guid;
                ForceDataRefreshOnNextAppearing();
            }
            else if (Guid.TryParse(idObj?.ToString(), out var parsed))
            {
                UserId = parsed;
                ForceDataRefreshOnNextAppearing();
            }
        }
    }

    // Partial callback invoked by CommunityToolkit when UserId changes
    partial void OnUserIdChanged(Guid value)
    {
        if (value != Guid.Empty)
        {
            ForceDataRefreshOnNextAppearing();
        }
    }

}
