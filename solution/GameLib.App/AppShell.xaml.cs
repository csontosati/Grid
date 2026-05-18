using GameLib.App.Views;

namespace GameLib.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(UserSettingsView), typeof(UserSettingsView));
        Routing.RegisterRoute(nameof(SignUpView), typeof(SignUpView));
        Routing.RegisterRoute(nameof(LibraryView), typeof(LibraryView));
        Routing.RegisterRoute(nameof(GameDetailView), typeof(GameDetailView));
        Routing.RegisterRoute(nameof(DiscoverView), typeof(DiscoverView));
        Routing.RegisterRoute(nameof(GameEditView), typeof(GameEditView));

    }
}