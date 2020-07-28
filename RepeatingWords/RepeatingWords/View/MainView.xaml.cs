using FormsControls.Base;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;

namespace RepeatingWords.View
{
    public partial class MainView : IAnimationPage 
    {
        public MainView()
        {
           InitializeComponent();
           BindingContext = LocatorService.Container.GetInstance<MainViewModel>();
        }

        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromRight };
        public void OnAnimationStarted(bool isPopAnimation) { }
        public void OnAnimationFinished(bool isPopAnimation){ }
    }
}