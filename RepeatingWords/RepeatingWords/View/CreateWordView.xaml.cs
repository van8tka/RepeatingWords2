using Xamarin.Forms;
using RepeatingWords.Services;
using Unity;
using RepeatingWords.ViewModel;

namespace RepeatingWords.View
{
    public partial class CreateWordView : ContentPage
    {
        public CreateWordView( )
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.Resolve<CreateWordViewModel>();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            EntryNativeWord.Focus();
        }
    }
}