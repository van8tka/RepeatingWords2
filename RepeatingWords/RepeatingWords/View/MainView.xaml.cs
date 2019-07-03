using RepeatingWords.ViewModel;
using Xamarin.Forms;

namespace RepeatingWords
{
    public partial class MainView : ContentPage
    {
        public MainView()
        {
           InitializeComponent();        
        }

        [Unity.Dependency]
        public MainViewModel ViewModel
        {
            get => BindingContext as MainViewModel;
            set => BindingContext = value;
        }
    }
}