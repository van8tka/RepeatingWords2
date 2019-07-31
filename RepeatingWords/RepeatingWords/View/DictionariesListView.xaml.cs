using Xamarin.Forms;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Unity;

namespace RepeatingWords.View
{
    public partial class DictionariesListView : ContentPage
    {
        public DictionariesListView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.Resolve<DictionariesListViewModel>();
        }
    }
}