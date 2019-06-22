using RepeatingWords.DataService.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Pages;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public interface IMainPage
    {
        ICommand BegianLearningCommand { get; set; }
        ICommand ChooseDictionaryCommand { get; set; }
        ICommand ContinueLearningCommand { get; set; }
        ICommand ShowToolsCommand { get; set; }
        ICommand LikeCommand { get; set; }
        ICommand HelperCommand { get; set; }
    }
    /// <summary>
    /// the first page of application
    /// </summary>
    public class MainPageVM:IMainPage, INotifyPropertyChanged
    {
        public MainPageVM(IUnitOfWork unitOfWork, INavigation navigation)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            ChooseDictionaryCommand = new Command(async() => {await _navigation.PushAsync(new ChooseDb()); });
            BegianLearningCommand = new Command(async () => { await _navigation.PushAsync(new ChooseDictionaryForRepiat()); });
            ContinueLearningCommand = new Command(ContinueLearning);
            ShowToolsCommand = new Command(async () => { await _navigation.PushAsync(new ToolsPage()); });
            HelperCommand = new Command(async () => { await _navigation.PushAsync(new Spravka()); });
            LikeCommand = new Command(LikeApplication);
        }

        
        private readonly IUnitOfWork _unitOfWork;
        private readonly INavigation _navigation;

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
                        await _navigation.PushAsync(adwords);                 
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
                bool action = await App.Current.MainPage.DisplayAlert("", MessagePleaseReview, ButtonSendReview, ButtonCancel);
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanges([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }

   
}
