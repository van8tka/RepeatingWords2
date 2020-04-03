using FormsControls.Base;
using RepeatingWords.Services;
using Xamarin.Forms;
 
using RepeatingWords.ViewModel;

namespace RepeatingWords.View
{
    public partial class RepeatingWordsView : IAnimationPage
    {
        public RepeatingWordsView()
        {
            InitializeComponent();
            var VM = LocatorService.Container.GetInstance<RepeatingWordsViewModel>();
            VM.WorkContainerView = cvWorkSpaceContainer;
            BindingContext = VM;
        }

        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromRight };
        public void OnAnimationStarted(bool isPopAnimation) { }
        public void OnAnimationFinished(bool isPopAnimation) { }
    }
}