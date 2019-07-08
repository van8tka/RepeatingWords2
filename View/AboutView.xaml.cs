
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms;
using Unity;
namespace RepeatingWords.View
{

    public partial class AboutView : ContentPage
    {
        public AboutView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.Resolve<AboutViewModel>();
        }

       
    }

}
