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

        Navigated += OnNavigated;
    }

    private async void OnNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        if (e.Current.Location.OriginalString.Contains("LibraryView"))
        {
            await _viewModel.OnAppearingAsync();
        }
    }
}