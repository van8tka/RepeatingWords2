using FormsControls.Base;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms;
 
namespace RepeatingWords.View
{

    public partial class SettingsView : IAnimationPage
    {
        public SettingsView()
        {
            InitializeComponent();
            //fullscreen advertizing
            DependencyService.Get<IAdmobInterstitial>().Show("ca-app-pub-5993977371632312/4024752876");
            BindingContext = LocatorService.Container.GetInstance<SettingsViewModel>();
        }
        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromRight };
        public void OnAnimationStarted(bool isPopAnimation) { }
        public void OnAnimationFinished(bool isPopAnimation) { }
    }

}
