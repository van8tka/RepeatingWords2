using System.Diagnostics;
using System.Threading.Tasks;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.ViewModel
{
    public class RepeatingWordsViewModel : ViewModelBase
    {
        public RepeatingWordsViewModel(INavigationService navigationServcie, IDialogService dialogService) : base(navigationServcie, dialogService)
        {
            Debugger.Break();
        }

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            return base.InitializeAsync(navigationData);
        }
    }
}
