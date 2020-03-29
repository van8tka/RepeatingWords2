using System;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Log = RepeatingWords.LoggerService.Log;


namespace RepeatingWords.ViewModel
{
    public abstract class WorkSpaceBaseViewModel: INotifyPropertyChanged, ICustomContentViewModel
    {
        private string _currentShowingWord;
        public string CurrentShowingWord { get => _currentShowingWord; set { _currentShowingWord = value; OnPropertyChanged(nameof(CurrentShowingWord)); } }
        private RepeatingWordsModel _model;
        public RepeatingWordsModel Model { get => _model; set { _model = value; OnPropertyChanged(nameof(Model)); } }
        public Xamarin.Forms.View WordContainer { get; set; }
       

        public WorkSpaceBaseViewModel(IDialogService _dialogService, INavigationService _navigationService,
            IUnlearningWordsService _unlearningWords, IAnimationService _animationService)
        {
            this._dialogService = _dialogService;
            this._navigationService = _navigationService;
            this._unlearningWords = _unlearningWords;
            this.AnimationService = _animationService;
        }
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IUnlearningWordsService _unlearningWords;
        protected readonly IAnimationService AnimationService;

        public async Task ShowNextWord(bool isFirstShowAfterLoad = false)
        {
            try
            {
                // await SaveUnlearnedWords(Model.CurrentWord, Model.IsOpenCurrentWord);
                Model.IsOpenCurrentWord = false;
                if (isFirstShowAfterLoad && Model.IndexWordShowNow != -1)
                    Model.IndexWordShowNow--;
                if ((Model.IndexWordShowNow < Model.AllWordsCount - 1 && Model.IndexWordShowNow >= 0) ||isFirstShowAfterLoad)
                {
                    Model.IndexWordShowNow++;
                    Model.CurrentWord = Model.WordsLearningAll.ElementAtOrDefault(Model.IndexWordShowNow);
                    await SetViewWords(Model.CurrentWord, Model.IsFromNative);
                    CounterShowWord(isFirstShowAfterLoad);
                    Model.WordsLeft.Remove(Model.CurrentWord);
                }
                else
                {
                    CounterShowWord(isFirstShowAfterLoad);
                    await _dialogService.ShowAlertDialog(Resource.ModalFinishWords, Resource.Continue);
                    await _navigationService.GoBackPage();
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private void CounterShowWord(bool isFirstShowAfterLoad)
        {
            if (!isFirstShowAfterLoad)
            {
                Model.AllShowedWordsCount++;
                Model.AllLearnedWordsCount = Model.AllShowedWordsCount - Model.AllOpenedWordsCount;
            }
        }

        /// <summary>
        /// если слово было открыто, не написано, выбрано не правильно то сохраняем его в словаре не выученных(или не удаляем его от туда)
        /// иначе если слово было написано правильно, выбрано правильно или пролистано, то удаляем его из не выученных слов
        /// </summary>    
        //private async Task SaveUnlearnedWords(Words word,bool isOpenCurrentWord)
        //{
        //    if (word != null)
        //       await Task.Run(() => { _unlearningWords.CheckSaveOrRemoveWord(word, isOpenCurrentWord, Model.Dictionary.Name); });
        //    Model.IsOpenCurrentWord = false;
        //}
  
        public abstract Task SetViewWords(Words currentWord, bool isFromNative);

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
