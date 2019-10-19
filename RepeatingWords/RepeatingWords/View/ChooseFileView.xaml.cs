using FormsControls.Base;
using Xamarin.Forms;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;

namespace RepeatingWords.View
{
    public partial class ChooseFileView : ContentPage, IAnimationPage
    {
        public ChooseFileView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.GetInstance<ChooseFileViewModel>();
        }
        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromRight };
        public void OnAnimationStarted(bool isPopAnimation) { }
        public void OnAnimationFinished(bool isPopAnimation) { }
    }
}