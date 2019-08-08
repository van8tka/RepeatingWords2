using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RepeatingWords.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LearningCardsView : ContentView
    {
        public LearningCardsView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.Resolve<LearningCardsViewModel>();
        }
    }
}