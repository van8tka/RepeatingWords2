using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RepeatingWords.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LearningCardsView : ContentView, ICustomContentView
    {
        public LearningCardsView()
        {
            InitializeComponent();
            customContentVM = LocatorService.Container.Resolve<LearningCardsViewModel>();
            BindingContext = CustomVM as LearningCardsViewModel;
        }
        private readonly ICustomContentViewModel customContentVM;
        public ICustomContentViewModel CustomVM => customContentVM;
    }
}