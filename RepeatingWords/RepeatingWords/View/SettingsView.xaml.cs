using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms;
 
namespace RepeatingWords.View
{

    public partial class SettingsView : ContentPage
    {
        public SettingsView()
        {
            InitializeComponent();
            //fullscreen advertizing
            DependencyService.Get<IAdmobInterstitial>().Show("ca-app-pub-5993977371632312/4024752876");
            BindingContext = LocatorService.Container.GetInstance<SettingsViewModel>();
        }   
    }

}
