using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class WorkSpaceCardsViewModel : INotifyPropertyChanged, ICustomContentViewModel
    {
        public WorkSpaceCardsViewModel()
        {
            SwipeWordCommand = new Command<string>((direction) => { SwipeWord(direction); });
        }
        private bool _isOpened = false;
        private int _indexWordShowNow = -1;

        public ICommand SwipeWordCommand { get; set; }
        private bool _isTranscriptionShow;
        public bool IsTranscriptionShow { get => _isTranscriptionShow; set { _isTranscriptionShow = value; OnPropertyChanged(nameof(IsTranscriptionShow)); } }
        private string _currentShowingWord;
        public string CurrentShowingWord { get => _currentShowingWord; set { _currentShowingWord = value; OnPropertyChanged(nameof(CurrentShowingWord)); } }
        private string _currentTranscriptWord;
        public string CurrentTranscriptWord { get => _currentTranscriptWord; set { _currentTranscriptWord = value; OnPropertyChanged(nameof(CurrentTranscriptWord)); } }


        private RepeatingWordsModel _model;
        public RepeatingWordsModel Model {
            get =>_model;
            set {
                _model = value;
                OnPropertyChanged(nameof(Model)); } }


        private void SwipeWord(string direction)
        {
            var enumDirection = (SwipeDirection)Enum.Parse(typeof(SwipeDirection), direction);
            switch (enumDirection)
            {
                case SwipeDirection.Down:
                case SwipeDirection.Up:
                    {
                        ShowTranslateWord();
                        break;
                    }
                case SwipeDirection.Left:
                    {
                        ShowNextWord();
                        break;
                    }
                case SwipeDirection.Right:
                    {
                        ShowPreviousWord();
                        break;
                    }
            }
        }

       
        internal void ShowNextWord(bool isFirstShowAfterLoad = false)
        {
            _isOpened = false;
            if (_indexWordShowNow < Model.wordsCollection.Count() - 1 && _indexWordShowNow >= 0 || isFirstShowAfterLoad)
            {
                _indexWordShowNow++;
                Model.currentWord = Model.wordsCollection.ElementAt(_indexWordShowNow);
                SetViewWords(Model.currentWord, Model.isFromNative);
                Model.AllShowedWordsCount++;
                Model.wordsCollectionLeft.Remove(Model.currentWord);
            }
        }

        private void SetViewWords(Words word, bool isNative, bool isOpened = false)
        {
            if (isOpened)
                isNative = !isNative;
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
            if (_indexWordShowNow > 0)
            {
                _indexWordShowNow--;
                Model.currentWord = Model.wordsCollection.ElementAt(_indexWordShowNow);
                SetViewWords(Model.currentWord, Model.isFromNative);
                Model.AllShowedWordsCount--;
                Model.wordsCollectionLeft.Add(Model.currentWord);
            }
            else
                _indexWordShowNow = 0;
        }

      
        private void ShowTranslateWord()
        {
            SetOpenWordsCount();
            _isOpened = !_isOpened;
            SetViewWords(Model.currentWord, Model.isFromNative, _isOpened);

        }


        private void SetOpenWordsCount()
        {
            if (!Model.wordsOpen.Contains(Model.currentWord))
            {
                Model.wordsOpen.Add(Model.currentWord);
                Model.AllOpenedWordsCount++;
            }
        }
       



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName=null )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
