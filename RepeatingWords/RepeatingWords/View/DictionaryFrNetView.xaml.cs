using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms;
using Unity;
namespace RepeatingWords.View
{
    public partial class DictionaryFrNetView : ContentPage
    {
        public DictionaryFrNetView()
        {
            InitializeComponent();          
            BindingContext = LocatorService.Container.Resolve<DictionaryFrNetViewModel>();
        }
    }
}