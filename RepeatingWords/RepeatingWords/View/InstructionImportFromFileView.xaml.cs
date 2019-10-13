using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms;
 
namespace RepeatingWords.View
{
    public partial class InstructionImportFromFileView : ContentPage
    {
        public InstructionImportFromFileView()
        {
            InitializeComponent();
            BindingContext = LocatorService.Container.GetInstance<InstructionImportFromFileViewModel>();
        }
    }
}
