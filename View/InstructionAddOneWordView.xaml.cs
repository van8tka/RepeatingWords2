using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Unity;
using Xamarin.Forms;

namespace RepeatingWords.View
{
    public partial class InstructionAddOneWordView : ContentPage
    {
        public InstructionAddOneWordView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.Resolve<InstructionAddOneWordViewModel>(); 
        }       
    }
}
