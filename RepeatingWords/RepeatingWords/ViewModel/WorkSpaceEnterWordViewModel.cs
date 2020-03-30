using RepeatingWords.DataService.Model;
using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Model;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class WorkSpaceEnterWordViewModel : WorkSpaceBaseViewModel
    {
        public WorkSpaceEnterWordViewModel(IDialogService _dialogService, INavigationService _navigationService, IAnimationService animation, IEntryWordValidator wordValidator) : base(_dialogService, _navigationService, animation)
        {
            CheckWordCommand = new Command(async () => await CheckWord());
            HintWordCommand = new Command(async () => await HintWord());
            ColorEnterWord = Color.LightGray;
            _wordValidator = wordValidator ?? throw new ArgumentNullException(nameof(wordValidator));
        }

        private WordsModel _showingWord;     
        private int _countCheckAvailabel = Constants.CHECK_AVAILABLE_COUNT;
        private readonly IEntryWordValidator _wordValidator;
        private string _enterAnswerWord;
        public string EnterAnswerWord
        {
            get => _enterAnswerWord; set
            {
                _enterAnswerWord = value; OnPropertyChanged(nameof(EnterAnswerWord));
            }
        }

        private Color _colorEnterWord;
        public Color ColorEnterWord { get => _colorEnterWord; set { _colorEnterWord = value; OnPropertyChanged(nameof(ColorEnterWord)); } }


        public ICommand CheckWordCommand { get; set; }
        public ICommand HintWordCommand { get; set; }

        public override async Task SetViewWords(WordsModel currentWord, bool isFromNative)
        {
            await AnimationService.AnimationFade(WordContainer,0);
            _showingWord = currentWord;
            CurrentShowingWord = isFromNative ? currentWord.RusWord : currentWord.EngWord;
            EnterAnswerWord = string.Empty;
            await AnimationService.AnimationFade(WordContainer, 1);
        }

        /// <summary>
        /// подсказывать слово
        /// </summary>
        /// <returns></returns>
        private async Task HintWord()
        {
            try
            {
                var checkWord = Model.IsFromNative ? _showingWord.EngWord : _showingWord.RusWord;
                if (string.Equals(EnterAnswerWord, checkWord, StringComparison.OrdinalIgnoreCase))
                {
                    await SetValidMarks();
                    _countCheckAvailabel = Constants.CHECK_AVAILABLE_COUNT; ;
                    IncrementOpenWords();
                    Model.IsOpenCurrentWord = false;
                    await ShowNextWord();
                }
                else
                {
                    CompareEntryWord(checkWord);
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private void CompareEntryWord(string checkWord)
        {
            StringBuilder tempEnterWord;
            if (EnterAnswerWord.Count() > checkWord.Count())
                tempEnterWord = new StringBuilder(EnterAnswerWord.Remove(checkWord.Count()));
            else
                tempEnterWord = new StringBuilder(EnterAnswerWord);
            SetHintChar(checkWord, tempEnterWord);
        }

        private void SetHintChar(string checkWord, StringBuilder tempEnterWord)
        {
            try
            {
                if (string.Equals(tempEnterWord.ToString(), checkWord, StringComparison.OrdinalIgnoreCase))
                {
                    EnterAnswerWord = tempEnterWord.ToString();
                    IncrementOpenWords();
                }
                else
                {
                    ComparerCharsWordEntryAndOriginalWord(checkWord, tempEnterWord);
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private void ComparerCharsWordEntryAndOriginalWord(string checkWord, StringBuilder tempEnterWord)
        {
            try
            {
                bool isNotSetHint = true;
                for (int i = 0; i < tempEnterWord.Length; i++)
                {
                    if (!Char.Equals(tempEnterWord[i], checkWord[i]))
                    {
                        tempEnterWord[i] = checkWord[i];
                        if(i==0)
                            EnterAnswerWord = checkWord[i].ToString(); 
                        else
                            EnterAnswerWord = tempEnterWord.ToString().Remove(i);
                        isNotSetHint = false;
                        break;
                    }
                }
                if (isNotSetHint)
                    EnterAnswerWord = tempEnterWord.Append(checkWord[tempEnterWord.Length]).ToString();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private async Task CheckWord()
        {
            try
            {
                var checkWord = Model.IsFromNative ? _showingWord.EngWord : _showingWord.RusWord;
                if ( _wordValidator.IsValidWord(EnterAnswerWord, checkWord))
                {                 
                    await SetValidMarks();
                    _countCheckAvailabel = Constants.CHECK_AVAILABLE_COUNT;
                    Model.IsOpenCurrentWord = false;
                    await ShowNextWord();
                }
                else
                {
                    EnterAnswerWord = _wordValidator.ClearEntryWord(EnterAnswerWord, checkWord);
                    await SetInvalidMarks();
                    _countCheckAvailabel--;
                }
                await UnavailableCheck();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private async Task UnavailableCheck()
        {
            if (_countCheckAvailabel == 0)
            {             
                await ShowRightWord();
                IncrementOpenWords();
                _countCheckAvailabel = Constants.CHECK_AVAILABLE_COUNT;
                Model.IsOpenCurrentWord = true;
                ShowNextWord();
            }
        }

        private async Task ShowRightWord()
        {
            EnterAnswerWord = Model.IsFromNative ? _showingWord.EngWord : _showingWord.RusWord;
            await SetValidMarks();
        }

        private void IncrementOpenWords()
        {
            Model.WordsOpen.Add(_showingWord);
            Model.AllOpenedWordsCount++;
        }
        private async Task SetInvalidMarks()
        {
            ColorEnterWord = Color.Red;           
            await Task.Delay(800);
           ColorEnterWord = Color.LightGray;
        }
        private async Task SetValidMarks()
        {
            ColorEnterWord = new Color(0.42, 0.69, 0.94, 1.0);         
            await Task.Delay(800);
            ColorEnterWord = Color.LightGray;
        }      
    }
}
