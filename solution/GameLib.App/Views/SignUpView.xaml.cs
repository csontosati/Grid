using GameLib.App.ViewModels;

namespace GameLib.App.Views;

public partial class SignUpView : ContentPageBase
{
    public SignUpView(UserAddViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }
}