using RepeatingWords.Helpers.Interfaces;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class AboutViewModel:ViewModelBase
    {
        public AboutViewModel(INavigationService navi, IDialogService dial):base(navi,dial){   }
      
        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;          
            VersionApp = ("Cards of words v" + DependencyService.Get<IVersionApp>().GetVersionApp()).Replace(',', '.');
            return base.InitializeAsync(navigationData);
        }

        private string _versionApp;
        public string VersionApp { get => _versionApp;
            set { _versionApp = value; OnPropertyChanged(nameof(VersionApp)); }
        }
    }
}
