using System.Threading.Tasks;
using FormsControls.Base;
using Xamarin.Forms;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;

namespace RepeatingWords.View
{
    public partial class CreateWordView : ContentPage, IAnimationPage
    {
        public CreateWordView( )
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.GetInstance<CreateWordViewModel>();
            SetFocus();
        }
        private void SetFocus()
        {
           MessagingCenter.Subscribe<CreateWordViewModel>(this,"SetFocus", async (sender) =>
           {
               await Task.Delay(10);
               EntryNativeWord.Focus();
           });
        }

        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromRight };
        public void OnAnimationStarted(bool isPopAnimation) { }
        public void OnAnimationFinished(bool isPopAnimation) { }
    }
}