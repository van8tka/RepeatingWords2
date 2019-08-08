using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RepeatingWords.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VolumeLanguagesView : ContentPage
    {
        public VolumeLanguagesView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.Resolve<VolumeLanguagesViewModel>();
        }
    }
}