using GameLib.App.ViewModels;

namespace GameLib.App.Views;

public partial class SignUpView : ContentPage
{
    public SignUpView()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current!.Services.GetService<UserAddViewModel>();
    }
}