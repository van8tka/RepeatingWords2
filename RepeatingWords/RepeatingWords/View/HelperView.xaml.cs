using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms;
 

namespace RepeatingWords.View
{
    public partial class HelperView : ContentPage
    {
        public HelperView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.GetInstance<HelperViewModel>();
        }
    }
}
