using Xamarin.Forms;
using RepeatingWords.Services;
using Unity;
using RepeatingWords.ViewModel;

namespace RepeatingWords.View
{
    public partial class WordsListView : ContentPage
    {
   
        public WordsListView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.Resolve<WordsListViewModel>();
        }    
    }
}