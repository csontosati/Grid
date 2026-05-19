using GameLib.App.ViewModels;

namespace GameLib.App;

public partial class AppShell : Shell
{
    private readonly AppShellViewModel _viewModel;

    public AppShell(AppShellViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;

        Routing.RegisterRoute(nameof(Views.UserSettingsView), typeof(Views.UserSettingsView));
        Routing.RegisterRoute(nameof(Views.SignUpView), typeof(Views.SignUpView));
        Routing.RegisterRoute(nameof(Views.LibraryView), typeof(Views.LibraryView));
        Routing.RegisterRoute(nameof(Views.GameDetailView), typeof(Views.GameDetailView));
        Routing.RegisterRoute(nameof(Views.DiscoverView), typeof(Views.DiscoverView));
        Routing.RegisterRoute(nameof(Views.GameEditView), typeof(Views.GameEditView));

        Navigated += OnNavigated;
    }

    private async void OnNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        if (e.Current.Location.OriginalString.Contains("LibraryView"))
        {
            await Task.Delay(100);
            _viewModel.ForceDataRefresh();
            await _viewModel.OnAppearingAsync();
        }
    }
}