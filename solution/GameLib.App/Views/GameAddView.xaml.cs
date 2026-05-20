using GameLib.App.ViewModels;
using GameLib.BL.Models;

namespace GameLib.App.Views;

public partial class GameAddView : ContentPageBase
{
    public GameAddView(GameAddViewModel viewModel) : base(viewModel)
    {
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
        InitializeComponent();
    }
}