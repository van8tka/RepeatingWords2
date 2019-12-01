using Microsoft.EntityFrameworkCore.Internal;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class WorkSpaceSelectWordViewModel : WorkSpaceBaseViewModel
    {
        public WorkSpaceSelectWordViewModel(IDialogService _dialogService, INavigationService _navigationService, IUnlearningWordsService unlearningManager, IAnimationService animation) : base(_dialogService, _navigationService, unlearningManager, animation)
        {
            TapWordCommand = new Command<string>(TapWord);
        }

        private int _timeShowRightWord = 800;
        private int _timeShowMistakeWord = 3500;
        /// <summary>
        /// действие по тапу на квадратик со словом
        /// </summary>
        /// <param name="wordName">слово выбранное в квадрате</param>
        private bool isTapAlready;
        private async void TapWord(string wordName)
        {
            if(!isTapAlready)
            {
                isTapAlready = true;
                string compareWord = !Model.IsFromNative ? _showingWord.RusWord : _showingWord.EngWord;
                if (string.IsNullOrEmpty(wordName))
                    return;
                if (compareWord.Equals(wordName, StringComparison.OrdinalIgnoreCase))
                {
                    Model.IsOpenCurrentWord = false;
                    SetRightMark(wordName, Color.FromHex("#6bafef"));
                    await Task.Delay(_timeShowRightWord);
                }
                else
                {
                    Model.IsOpenCurrentWord = true;
                    SetRightMark(compareWord, Color.FromHex("#6bafef"));
                    SetRightMark(wordName, Color.Red);
                    Model.WordsOpen.Add(_showingWord);
                    Model.AllOpenedWordsCount++;
                    await Task.Delay(_timeShowMistakeWord);
                }
                ClearBackgroundColor();
                await AnimationService.AnimationFade(WordContainer, 0);
                ShowNextWord();
                await AnimationService.AnimationFade(WordContainer, 1);
                isTapAlready = false;
            }           
        }

        private void ClearBackgroundColor()
        {
            FirstBackgroundColor = Color.Transparent;
            SecondBackgroundColor = Color.Transparent;
            ThirdBackgroundColor = Color.Transparent;
            ForthBackgroundColor = Color.Transparent;
        }
        /// <summary>
        /// установка цвета фона квадрата при отганном или неотгаданном слове
        /// </summary>       
        private void SetRightMark(string word, Color color)
        {
            if (string.Equals(word, FirstWord, StringComparison.OrdinalIgnoreCase))
            {
                FirstBackgroundColor = color;
            }
            else
            if (string.Equals(word, SecondWord, StringComparison.OrdinalIgnoreCase))
            {
                SecondBackgroundColor = color;
            }
            else
            if (string.Equals(word, ThirdWord, StringComparison.OrdinalIgnoreCase))
            {
                ThirdBackgroundColor = color;
            }
            else
            if (string.Equals(word, ForthWord, StringComparison.OrdinalIgnoreCase))
            {
                ForthBackgroundColor = color;
            }
            else
                throw new ArgumentException(nameof(word));
        }

        public ICommand TapWordCommand { get; set; }

        private Color _firstbackgroundColor;
        public Color FirstBackgroundColor { get=> _firstbackgroundColor; set { _firstbackgroundColor = value; OnPropertyChanged(nameof(FirstBackgroundColor)); } }
        private Color _secondbackgroundColor;
        public Color SecondBackgroundColor { get => _secondbackgroundColor; set { _secondbackgroundColor = value; OnPropertyChanged(nameof(SecondBackgroundColor)); } }
        private Color _thirdbackgroundColor;
        public Color ThirdBackgroundColor { get => _thirdbackgroundColor; set { _thirdbackgroundColor = value; OnPropertyChanged(nameof(ThirdBackgroundColor)); } }
        private Color _forthbackgroundColor;
        public Color ForthBackgroundColor { get => _forthbackgroundColor; set { _forthbackgroundColor = value; OnPropertyChanged(nameof(ForthBackgroundColor)); } }
 
       
        private string _firstWord;
        public string FirstWord { get => _firstWord; set { _firstWord = value; OnPropertyChanged(nameof(FirstWord)); } }
        private string _secondWord;
        public string SecondWord { get => _secondWord; set { _secondWord = value; OnPropertyChanged(nameof(SecondWord)); } }
        private string _thirdWord;
        public string ThirdWord { get => _thirdWord; set { _thirdWord = value; OnPropertyChanged(nameof(ThirdWord)); } }
        private string _forthWord;
        public string ForthWord { get => _forthWord; set { _forthWord = value; OnPropertyChanged(nameof(ForthWord)); } }

       

        private Words _showingWord;
        internal override Task SetViewWords(Words currentWord, bool isFromNative)
        {
            _showingWord = currentWord;
            CurrentShowingWord = isFromNative ? currentWord.RusWord : currentWord.EngWord;
            SetSelectingWords(currentWord, isFromNative);
            return Task.Delay(1);
        }

        public void SetSelectingWords(Words word, bool isNative)
        {
           var otherWords = GetThreeWords(word); 
           if(otherWords.Any())
            {
                if (!isNative)
                {
                    FirstWord = otherWords[3].RusWord;
                    SecondWord = otherWords[0].RusWord;
                    ThirdWord = otherWords[1].RusWord;
                    ForthWord = otherWords[2].RusWord;
                }
                else
                {
                    FirstWord = otherWords[3].EngWord;
                    SecondWord = otherWords[0].EngWord;
                    ThirdWord = otherWords[1].EngWord;
                    ForthWord = otherWords[2].EngWord;
                }
            }           
        }

        private IList<Words> GetThreeWords(Words word)
        {
            var listWordsForGrid = new List<Words>(4);
            try
            {             
                var random = new Random();
                int border = Model.WordsLearningAll.Count() - 1;
                if (border < 3)
                    return listWordsForGrid;
                int idCurrent = Model.WordsLearningAll.IndexOf(word);
                int id1 = 0, id2 = 0, id3 = 0;
                while (idCurrent == id1 || idCurrent == id2 || idCurrent == id3 || id1 == id2 || id1==id3 || id3==id2)
                {
                    id1 = random.Next(0, border);
                    id2 = random.Next(0, border);
                    id3 = random.Next(0, border);
                }
                listWordsForGrid.Add(Model.WordsLearningAll.ElementAt(id1));
                listWordsForGrid.Add(Model.WordsLearningAll.ElementAt(id2));
                listWordsForGrid.Add(Model.WordsLearningAll.ElementAt(id3));
                int idMainWord = random.Next(0, 3);
                listWordsForGrid.Insert(idMainWord, word);
                return listWordsForGrid;
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
                return listWordsForGrid;
            }            
        }
    }
}
