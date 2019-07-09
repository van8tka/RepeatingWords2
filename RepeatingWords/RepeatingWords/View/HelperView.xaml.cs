using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms;
using Unity;

namespace RepeatingWords.View
{
    public partial class HelperView : ContentPage
    {
        public HelperView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.Resolve<HelperViewModel>();
        }
    }
}
