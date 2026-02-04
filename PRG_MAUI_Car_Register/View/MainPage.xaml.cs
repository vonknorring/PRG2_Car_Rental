using PRG_MAUI_Car_Register.ViewModel;
using System.Windows.Input;

namespace PRG_MAUI_Car_Register
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }
    }
}
