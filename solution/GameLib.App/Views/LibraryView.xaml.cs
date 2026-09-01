using GameLib.App.ViewModels;

namespace GameLib.App.Views;

public partial class LibraryView : ContentPageBase
{
	public LibraryView(LibraryListViewModel viewModel) : base(viewModel)
	{
		Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
		InitializeComponent();
	}
}