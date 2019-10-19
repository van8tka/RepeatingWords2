using FormsControls.Base;
using RepeatingWords.Services;
using Xamarin.Forms;
 
using RepeatingWords.ViewModel;

namespace RepeatingWords.View
{
    public partial class RepeatingWordsView : ContentPage, IAnimationPage
    {
        public RepeatingWordsView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.GetInstance<RepeatingWordsViewModel>();
        }

        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromRight };
        public void OnAnimationStarted(bool isPopAnimation) { }
        public void OnAnimationFinished(bool isPopAnimation) { }
    }
}