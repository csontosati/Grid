using GameLib.App.ViewModels;

namespace GameLib.App.Views;

public partial class UserSettingsView : ContentPageBase
{
    public UserSettingsView(UserSettingsViewModel viewModel) : base(viewModel)
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
        InitializeComponent();
    }
}