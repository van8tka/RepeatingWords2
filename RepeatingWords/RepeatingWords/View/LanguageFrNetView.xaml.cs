using FormsControls.Base;
using RepeatingWords.Services;
using Xamarin.Forms;

using RepeatingWords.ViewModel;

namespace RepeatingWords.View
{
    public partial class LanguageFrNetView : ContentPage, IAnimationPage
    {            
        public LanguageFrNetView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.GetInstance<LanguageFrNetViewModel>(); 
        }
        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromRight };
        public void OnAnimationStarted(bool isPopAnimation) { }
        public void OnAnimationFinished(bool isPopAnimation) { }
    }
}
