using RepeatingWords.DataService.Interfaces;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{

    /// <summary>
    /// the first page of application
    /// </summary>
    public class MainViewModel : ViewModelBase, IMainPage
    {
        public MainViewModel(IUnitOfWork unitOfWork, INavigationService navService, IDialogService dialogService) : base(navService, dialogService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            ChooseDictionaryCommand = new Command(async () => { await NavigationService.NavigateToAsync<DictionariesListViewModel>(); });
            ContinueLearningCommand = new Command(ContinueLearning);
            ShowToolsCommand = new Command(async () => { await NavigationService.NavigateToAsync<SettingsViewModel>(); });
            HelperCommand = new Command(async () => { await NavigationService.NavigateToAsync<HelperViewModel>(); });
            LikeCommand = new Command(LikeApplication);
            LoadPageCommand = new Command(AppearingPage);
        }

        private async void AppearingPage( )
        {
           await CheckIsEnabledContinue();
        }

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;          
            return base.InitializeAsync(navigationData);
        }

         

        private async Task CheckIsEnabledContinue()
        {
            var item = await Task.Run(() => _unitOfWork.LastActionRepository.Get().LastOrDefault());
            IsEnabledContinue = item != null ? true : false;          
        }

        private readonly IUnitOfWork _unitOfWork;

        private bool _isEnabledContinue;
        public bool IsEnabledContinue { get => _isEnabledContinue; set { _isEnabledContinue = value; OnPropertyChanged(nameof(IsEnabledContinue)); } }

        public ICommand BegianLearningCommand { get; set; }
        public ICommand ChooseDictionaryCommand { get; set; }
        public ICommand ContinueLearningCommand { get; set; }
        public ICommand ShowToolsCommand { get; set; }
        public ICommand LikeCommand { get; set; }
        public ICommand HelperCommand { get; set; }
        public ICommand LoadPageCommand { get; set; }

        private async void ContinueLearning()
        {
            try
            {
                var lastAction = _unitOfWork.LastActionRepository.Get().LastOrDefault();
                await NavigationService.NavigateToAsync<RepeatingWordsViewModel>(lastAction);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private async void LikeApplication()
        {
            try
            {
                string MessagePleaseReview = Resource.MessagePleaseReview;
                string ButtonSendReview = Resource.ButtonSendReview;
                string ButtonCancel = Resource.ModalActCancel;
                bool action = await DialogService.ShowConfirmAsync(MessagePleaseReview, "", ButtonSendReview, ButtonCancel);
                if (action)
                {
                    if (Device.RuntimePlatform == "Android")
                        Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=cardsofwords.cardsofwords"));
                    //if (Device.RuntimePlatform == "UWP")
                    //    Device.OpenUri(new Uri("https://www.microsoft.com/store/apps/9n55bwkgshnf"));
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }


    }
}
