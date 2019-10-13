using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RepeatingWords.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkSpaceSelectWordView : ContentView, ICustomContentView
    {
        public WorkSpaceSelectWordView()
        {
            InitializeComponent();
            _customContentViewModel = LocatorService.Container.GetInstance<WorkSpaceSelectWordViewModel>();
            _customContentViewModel.WordContainer = stlWordContainer;
            BindingContext = _customContentViewModel as WorkSpaceSelectWordViewModel;
        }
        private ICustomContentViewModel _customContentViewModel;
        public ICustomContentViewModel CustomVM => _customContentViewModel;
    }
}