using RepeatingWords.ViewModel;
using Xamarin.Forms;

namespace RepeatingWords
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
           InitializeComponent();        
        }

        [Unity.Dependency]
        public MainPageVM ViewModel
        {
            get => BindingContext as MainPageVM;
            set => BindingContext = value;
        }
    }
}