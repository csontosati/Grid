using GameLib.App.Models;
using GameLib.App.PageModels;

namespace GameLib.App.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}