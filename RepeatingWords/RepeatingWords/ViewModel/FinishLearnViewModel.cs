using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
   public class FinishLearnViewModel:ViewModelBase
    {
        public FinishLearnViewModel(INavigationService navigationServcie, IDialogService dialogService, IAnimationService animationService) : base(navigationServcie, dialogService)
        {
            UnlearnedWordsCount = _unlearned;
            LearnedWordsCount = _learned;
            _animationService = animationService;
            GoMainPageCommand = new Command(GoMainPage);
        }

        private IAnimationService _animationService;
        internal Xamarin.Forms.View VCongratulation { get; set; }
        internal Xamarin.Forms.View VLearned { get; set; }
        internal Xamarin.Forms.View VUnlerned { get; set; }
        internal Xamarin.Forms.View VLearnMore { get; set; }


        public ICommand GoMainPageCommand { get; private set; }

        private async void GoMainPage()
        {
            await NavigationService.GoBackPage();
        }

        private string _learnedCount;
        public string LearnedWordsCount { get=>_learnedCount;
            set
            {
                _learnedCount = value; OnPropertyChanged(nameof(LearnedWordsCount));
            }
        }
        private string _unlearnedCount;

        public string UnlearnedWordsCount
        {
            get => _unlearnedCount;
            set
            {
                _unlearnedCount = value;
                OnPropertyChanged(nameof(UnlearnedWordsCount));
            }
        }
      
        private string _learned = Resource.LbLearnedCount;
        private string _unlearned = Resource.LbUnlearnedCount;

        public override async Task InitializeAsync(object navigatedata)
        {
            IsBusy = true;
            await NavigationService.RemoveLastFromBackStackAsync();
            if (navigatedata is Tuple<int, int> learnedData)
            {
                string lerned = _learnedCount + learnedData.Item1;
                string unlearned = _unlearned + learnedData.Item2;
                UnlearnedWordsCount = unlearned;
                LearnedWordsCount = lerned;
                await AnimationShowData();
            }
            else
                Log.Logger.Error("Error get count learned words");
            await base.InitializeAsync(navigatedata);
        }

        private async Task AnimationShowData()
        {
            await Task.Delay(250);
            await _animationService.AnimationFade(VCongratulation, 1, 250);
            await Task.Delay(350);
            await _animationService.AnimationFade(VLearned, 1, 250);
            await Task.Delay(250);
            await _animationService.AnimationFade(VUnlerned, 1,250);
            await Task.Delay(350);
            await _animationService.AnimationFade(VLearnMore, 1, 250);
            await Task.Delay(350);
            //fullscreen advertizing  
            DependencyService.Get<IAdmobInterstitial>().Show("ca-app-pub-5993977371632312/4024752876");
        }
    }
}
