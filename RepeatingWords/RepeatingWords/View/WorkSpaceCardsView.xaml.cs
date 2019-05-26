using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RepeatingWords.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkSpaceCardsView : ContentView, ICustomContentView
    {
        public WorkSpaceCardsView()
        {
            InitializeComponent();
            customContentVM = LocatorService.Container.GetInstance<WorkSpaceCardsViewModel>();
            BindingContext = CustomVM as WorkSpaceCardsViewModel;
        }
        private readonly ICustomContentViewModel customContentVM;
        public ICustomContentViewModel CustomVM => customContentVM;
    }
}