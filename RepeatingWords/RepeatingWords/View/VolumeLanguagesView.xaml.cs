using FormsControls.Base;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
 
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RepeatingWords.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VolumeLanguagesView : ContentPage, IAnimationPage
    {
        public VolumeLanguagesView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.GetInstance<VolumeLanguagesViewModel>();
        }
        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromRight };
        public void OnAnimationStarted(bool isPopAnimation) { }
        public void OnAnimationFinished(bool isPopAnimation) { }
    }
}