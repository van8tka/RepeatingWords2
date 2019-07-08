using System.Threading.Tasks;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.ViewModel
{
    public class InstructionAddOneWordViewModel:ViewModelBase
    {
        public InstructionAddOneWordViewModel(INavigationService navi, IDialogService dial):base(navi, dial)
        { }

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            return base.InitializeAsync(navigationData);
        }
    }
}
