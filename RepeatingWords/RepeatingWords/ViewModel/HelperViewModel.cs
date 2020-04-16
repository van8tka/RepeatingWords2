using RepeatingWords.Helpers.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class HelperViewModel : ViewModelBase 
    {
        //ctor
        public HelperViewModel(INavigationService navigation, IDialogService dialogService) : base(navigation, dialogService)
        {         
            HowToAddWordCommand = new Command(async()=> { await NavigationService.NavigateToAsync<InstructionAddOneWordViewModel>(); } );
            HowToImportFromFileCommand = new Command(async () => { await NavigationService.NavigateToAsync<InstructionImportFromFileViewModel>(); });
            AboutCommand = new Command(async()=> { await NavigationService.NavigateToAsync<AboutViewModel>(); });
            PolicyCommand = new Command(OpenPolicy);
        }

        private readonly string uriPolicy = "https://devprogram.ru/privacy.html";
        private async void OpenPolicy()
        {
            if (await Launcher.CanOpenAsync(uriPolicy))
                await Launcher.OpenAsync(uriPolicy);
        }
        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            return base.InitializeAsync(navigationData);
        }
        public ICommand HowToAddWordCommand { get; set; }
        public ICommand HowToImportFromFileCommand { get; set; }
        public ICommand AboutCommand { get; set; }
        public ICommand PolicyCommand { get; set; }
    }
}
