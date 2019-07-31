using Xamarin.Forms;
using RepeatingWords.Services;
using Unity;
using RepeatingWords.ViewModel;

namespace RepeatingWords.View
{
    public partial class ChooseFileView : ContentPage
    {
        public ChooseFileView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.Resolve<ChooseFileViewModel>();
        }       
    }
}