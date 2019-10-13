using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers;
using RepeatingWords.Helpers.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class WorkSpaceCardsViewModel : WorkSpaceBaseViewModel
    {
        public WorkSpaceCardsViewModel(IDialogService _dialogService, INavigationService _navigationService, IUnlearningWordsService unlearningManager, IAnimationService animation) : base(_dialogService, _navigationService, unlearningManager, animation)
        {
            SwipeWordCommand = new Command<string>((direction) => { SwipeWord(direction); });
        }
 

        private bool _isOpened = false;
        public ICommand SwipeWordCommand { get; set; }
        private bool _isTranscriptionShow;
        public bool IsTranscriptionShow { get => _isTranscriptionShow; set { _isTranscriptionShow = value; OnPropertyChanged(nameof(IsTranscriptionShow)); } }     
        private string _currentTranscriptWord;
        public string CurrentTranscriptWord { get => _currentTranscriptWord;
            set {
                if(TranscriptionChecker.CheckIsNotEmptyTranscription(value) )
                    _currentTranscriptWord = value;
                else
                    _currentTranscriptWord = string.Empty;
                OnPropertyChanged(nameof(CurrentTranscriptWord)); } }

      

        private async void SwipeWord(string direction)
        {
            var enumDirection = (SwipeDirection)Enum.Parse(typeof(SwipeDirection), direction);
            await AnimationService.AnimationPositionWord(enumDirection, WordContainer);
            switch (enumDirection)
            {
               
                case SwipeDirection.Down:
                case SwipeDirection.Up:
                    {
                        Model.IsOpenCurrentWord = true;
                        ShowTranslateWord();
                        break;
                    }
                case SwipeDirection.Left:
                    {                                          
                        ShowNextWord();
                        _isOpened = false;
                        break;
                    }
                case SwipeDirection.Right:
                    {
                        ShowPreviousWord();
                        break;
                    }
            }
            await AnimationService.AnimationPositionWordRevert(WordContainer);
        }

        internal override void SetViewWords(Words word, bool isNative)
        {
            SetViewWords( word, isNative, false);
        }

        private void SetViewWords(Words word, bool isNative, bool isOpened)
        {         
            if (isOpened)
            {               
                isNative = !isNative;
            }            
            if (isNative)
                CurrentShowingWord = word.RusWord;
            else
                CurrentShowingWord = word.EngWord;
            IsTranscriptionShow = !isNative;
            CurrentTranscriptWord = word.Transcription;
        }


        private void ShowPreviousWord()
        {
            _isOpened = false;
            if (Model.IndexWordShowNow > 0)
            {
                Model.IndexWordShowNow--;
                Model.CurrentWord = Model.WordsLearningAll.ElementAt(Model.IndexWordShowNow);
                SetViewWords(Model.CurrentWord, Model.IsFromNative);
                Model.AllShowedWordsCount--;
                Model.WordsLeft.Add(Model.CurrentWord);
            }
            else
                Model.IndexWordShowNow = 0;
        }

      
        private void ShowTranslateWord()
        {
            SetOpenWordsCount();
            _isOpened = !_isOpened;
            SetViewWords(Model.CurrentWord, Model.IsFromNative, _isOpened);

        }

        private void SetOpenWordsCount()
        {
            if (!Model.WordsOpen.Contains(Model.CurrentWord))
            {
                Model.WordsOpen.Add(Model.CurrentWord);
                Model.AllOpenedWordsCount++;              
            }
        }
    }
}
