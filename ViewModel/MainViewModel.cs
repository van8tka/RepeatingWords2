using RepeatingWords.DataService.Interfaces;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Services;
using RepeatingWords.View;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Unity;

namespace RepeatingWords.ViewModel
{
   
    /// <summary>
    /// the first page of application
    /// </summary>
    public class MainViewModel: ViewModelBase, IMainPage
    {
        public MainViewModel(IUnitOfWork unitOfWork, INavigationService navService, IDialogService dialogService):base(navService, dialogService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            //ChooseDictionaryCommand = new Command(async() => {await _navigation.PushAsync(new ChooseDb()); });
            //BegianLearningCommand = new Command(async () => { await _navigation.PushAsync(new ChooseDictionaryForRepiat()); });
            ContinueLearningCommand = new Command(ContinueLearning);
            ShowToolsCommand = new Command(async () => { await NavigationService.NavigateToAsync<SettingsViewModel>(); });
            HelperCommand = new Command(async () => { await NavigationService.NavigateToAsync<HelperViewModel>(); });           
            LikeCommand = new Command(LikeApplication);
        }

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            return base.InitializeAsync(navigationData);
        }



        private readonly IUnitOfWork _unitOfWork;
        

        public ICommand BegianLearningCommand { get ; set; }
        public ICommand ChooseDictionaryCommand { get ; set; }
        public ICommand ContinueLearningCommand { get ; set; }
        public ICommand ShowToolsCommand { get ; set; }
        public ICommand LikeCommand { get; set; }
        public ICommand HelperCommand { get; set; }

        private async void ContinueLearning()
        {
            try
            {              
                var lastAction = _unitOfWork.LastActionRepository.Get().LastOrDefault();
                if (lastAction != null)
                {                   
                        var adwords = new RepeatWord(lastAction);
                    Debugger.Break();
                       // await _navigation.PushAsync(adwords);                 
                }               
            }
            catch(Exception e)
            {
               Log.Logger.Error(e);
            }
        }

        private async void LikeApplication( )
        {
            try
            {
                string MessagePleaseReview = Resource.MessagePleaseReview;
                string ButtonSendReview = Resource.ButtonSendReview;
                string ButtonCancel = Resource.ModalActCancel;
                bool action = await DialogService.ShowConfirmAsync(MessagePleaseReview,"", ButtonSendReview, ButtonCancel);
                if (action)
                {
                    if (Device.RuntimePlatform == "Android")
                        Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=cardsofwords.cardsofwords"));
                    if (Device.RuntimePlatform == "UWP")                    
                        Device.OpenUri(new Uri("https://www.microsoft.com/store/apps/9n55bwkgshnf"));                   
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        
    }

   
}
