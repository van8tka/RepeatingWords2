using FormsControls.Base;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms;
 
namespace RepeatingWords.View
{
    public partial class InstructionImportFromFileView : ContentPage, IAnimationPage
    {
        public InstructionImportFromFileView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.GetInstance<InstructionImportFromFileViewModel>();
        }
        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromRight };
        public void OnAnimationStarted(bool isPopAnimation) { }
        public void OnAnimationFinished(bool isPopAnimation) { }
    }
}
