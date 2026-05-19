using GameLib.App.Services;
using GameLib.App.ViewModels;
using GameLib.BL.Facades;
using GameLib.BL.Models;
using System.Collections.ObjectModel;

public partial class AppShellViewModel(
    LibraryFacade libraryFacade,
    AppState appState,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public ObservableCollection<LibraryListModel> Libraries { get; } = new();

    protected override async Task LoadAsync()
    {
        if (appState.CurrentUserId == Guid.Empty) return;

        Libraries.Clear();
        var libs = await libraryFacade.GetByUserAsync(appState.CurrentUserId);
        foreach (var lib in libs.OrderBy(l => l.Name))
            Libraries.Add(lib);
    }
    public void ForceDataRefresh()
    {
        ForceDataRefreshOnNextAppearing();
    }
}