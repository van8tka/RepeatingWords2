using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Unity;
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
            _customContentViewModel = LocatorService.Container.Resolve<WorkSpaceSelectWordViewModel>();
            BindingContext = _customContentViewModel as WorkSpaceSelectWordViewModel;
        }
        private ICustomContentViewModel _customContentViewModel;
        public ICustomContentViewModel CustomVM => _customContentViewModel;
    }
}