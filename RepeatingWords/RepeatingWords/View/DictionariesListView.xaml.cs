using Xamarin.Forms;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
 

namespace RepeatingWords.View
{
    public partial class DictionariesListView : ContentPage
    {
        public DictionariesListView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.GetInstance<DictionariesListViewModel>();
        }
    }
}