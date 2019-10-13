using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using System.Threading.Tasks;

namespace RepeatingWords.ViewModel
{
    public class AboutViewModel:ViewModelBase
    {
        public AboutViewModel(INavigationService navi, IDialogService dial):base(navi,dial){   }
      
        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;          
            VersionApp = ("Cards of words v" + Constants.NUMBER_VERSION_ANDROID).Replace(',', '.');
            return base.InitializeAsync(navigationData);
        }

        private string _versionApp;
        public string VersionApp { get => _versionApp;
            set { _versionApp = value; OnPropertyChanged(nameof(VersionApp)); }
        }
    }
}
