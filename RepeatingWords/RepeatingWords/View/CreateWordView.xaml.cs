using Xamarin.Forms;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;

namespace RepeatingWords.View
{
    public partial class CreateWordView : ContentPage
    {
        public CreateWordView( )
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.GetInstance<CreateWordViewModel>();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            EntryNativeWord.Focus();
        }
    }
}