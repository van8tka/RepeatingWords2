using RepeatingWords.Services;
using Xamarin.Forms;
using Unity;
using RepeatingWords.ViewModel;

namespace RepeatingWords.View
{
    public partial class LanguageFrNetView : ContentPage
    {            
        public LanguageFrNetView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.Resolve<LanguageFrNetViewModel>(); 
        }    
    }
}
