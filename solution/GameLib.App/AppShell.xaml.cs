using GameLib.App.Messages;
using GameLib.App.Services;
using GameLib.App.ViewModels;
using GameLib.BL.Models;

namespace GameLib.App;

public partial class AppShell : Shell
{
    private readonly AppShellViewModel _viewModel;
    private readonly IMessengerService _messengerService;

    public AppShell(AppShellViewModel viewModel, IMessengerService messengerService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _messengerService = messengerService;
        BindingContext = viewModel;
        Navigated += OnNavigated;
        _viewModel.Libraries.CollectionChanged += OnLibrariesChanged;
    }

    private void OnLibrariesChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        RebuildLibraryFlyoutItems();
    }

    private void RebuildLibraryFlyoutItems()
    {
        var toRemove = Items
            .Where(i => i.AutomationId == "LibraryFlyoutItem")
            .ToList();

        foreach (var item in toRemove)
            Items.Remove(item);

        foreach (var library in _viewModel.Libraries)
        {
            var flyoutItem = new FlyoutItem
            {
                Title = library.Name,
                AutomationId = "LibraryFlyoutItem"
            };

            var shellContent = new ShellContent
            {
                ContentTemplate = new DataTemplate(typeof(Views.LibraryView))
            };

            flyoutItem.Items.Add(shellContent);
            flyoutItem.BindingContext = library;
            Items.Add(flyoutItem);
        }
    }

    private async void OnNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        var current = e.Current.Location.OriginalString;

        if (current.Contains("LibraryView"))
        {
            var selectedItem = Items
                .FirstOrDefault(i => i.AutomationId == "LibraryFlyoutItem" && i == CurrentItem);

            if (selectedItem?.BindingContext is LibraryListModel library)
            {
                _messengerService.Send(new LibrarySelectedMessage(library.Id));
            }

            await _viewModel.OnAppearingAsync();
        }
    }
}