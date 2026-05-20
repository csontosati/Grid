namespace GameLib.App.Views;

public partial class GameDetailView : ContentPage
{
	public GameDetailView()
	{
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
        InitializeComponent();
	}
}