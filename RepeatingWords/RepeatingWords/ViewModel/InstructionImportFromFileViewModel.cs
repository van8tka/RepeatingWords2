using RepeatingWords.Helpers.Interfaces;
using System.Threading.Tasks;
using RepeatingWords.Interfaces;

namespace RepeatingWords.ViewModel
{
    public class InstructionImportFromFileViewModel:ViewModelBase
    {
        public InstructionImportFromFileViewModel(INavigationService navi, IDialogService dial):base(navi, dial)
        { }
        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            return base.InitializeAsync(navigationData);
        }
    }
}
