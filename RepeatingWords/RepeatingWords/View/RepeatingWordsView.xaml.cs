using RepeatingWords.Services;
using Xamarin.Forms;
using Unity;
using RepeatingWords.ViewModel;

namespace RepeatingWords.View
{
    public partial class RepeatingWordsView : ContentPage
    {
        public RepeatingWordsView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.Resolve<RepeatingWordsViewModel>();
        }       

    }
}