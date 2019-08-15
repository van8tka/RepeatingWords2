using RepeatingWords.DataService.Model;
using RepeatingWords.LoggerService;
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
        public WorkSpaceEnterWordViewModel()
        {
            CheckWordCommand = new Command(async () => await CheckWord());
            HintWordCommand = new Command(async () => await HintWord());
        }

        private Words _showingWord;     
        private int _countCheckAvailabel = 3;

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

        internal override void SetViewWords(Words currentWord, bool isFromNative)
        {
            _showingWord = currentWord;
            CurrentShowingWord = isFromNative ? currentWord.RusWord : currentWord.EngWord;
            EnterAnswerWord = string.Empty;
        }


        private async Task HintWord()
        {
            try
            {
                var checkWord = Model.isFromNative ? _showingWord.EngWord : _showingWord.RusWord;
                if (string.Equals(EnterAnswerWord, checkWord, StringComparison.OrdinalIgnoreCase))
                {
                    await SetValidMarks();
                    _countCheckAvailabel = 3;
                    IncrementOpenWords();
                    ShowNextWord();
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
                var checkWord = Model.isFromNative ? _showingWord.EngWord : _showingWord.RusWord;
                if (string.Equals(EnterAnswerWord, checkWord, StringComparison.OrdinalIgnoreCase))
                {
                    await SetValidMarks();
                    _countCheckAvailabel = 3;
                    ShowNextWord();
                }
                else
                {
                    await SetInvalidMarks();
                    _countCheckAvailabel--;
                }
                UnavailableCheck();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private void UnavailableCheck()
        {
            if (_countCheckAvailabel == 0)
            {
                IncrementOpenWords();
                _countCheckAvailabel = 3;
                ShowNextWord();
            }
        }

        private void IncrementOpenWords()
        {
            Model.wordsOpen.Add(_showingWord);
            Model.AllOpenedWordsCount++;
        }
        private async Task SetInvalidMarks()
        {
            ColorEnterWord = Color.Red;
            await Task.Delay(800);
            ColorEnterWord = Color.Transparent;
        }
        private async Task SetValidMarks()
        {
            ColorEnterWord = Color.LightBlue;
            await Task.Delay(800);
            ColorEnterWord = Color.Transparent;
        }      
    }
}
