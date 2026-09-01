using CommunityToolkit.Mvvm.ComponentModel;
using GameLib.App.Services;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel; // ak potrebuješ MainThread

namespace GameLib.App.ViewModels;

public abstract class ViewModelBase : ObservableRecipient
{
    private bool _forceDataRefresh = true;
    private bool _initialized;

    protected readonly IMessengerService MessengerService;

    protected ViewModelBase(IMessengerService messengerService)
        : base(messengerService.Messenger)
    {
        MessengerService = messengerService;
        IsActive = true;
    }

    public async Task OnAppearingAsync()
    {
        if (_forceDataRefresh)
        {
            await LoadAsync();
            _forceDataRefresh = false;
        }
    }

    protected void ForceDataRefreshOnNextAppearing()
    {
        _forceDataRefresh = true;
    }


    protected virtual Task LoadAsync()
        => Task.CompletedTask;
}