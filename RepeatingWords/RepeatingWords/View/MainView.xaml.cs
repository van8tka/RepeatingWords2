using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms;
 
namespace RepeatingWords.View
{
    public partial class MainView : ContentPage
    {
        public MainView()
        {
           InitializeComponent();
           BindingContext = LocatorService.Container.GetInstance<MainViewModel>();
        }
    }
}