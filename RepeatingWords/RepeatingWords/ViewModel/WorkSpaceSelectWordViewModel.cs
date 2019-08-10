﻿using Microsoft.EntityFrameworkCore.Internal;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class WorkSpaceSelectWordViewModel : INotifyPropertyChanged, ICustomContentViewModel
    {
        public WorkSpaceSelectWordViewModel()
        {
            TapWordCommand = new Command<string>(TapWord);
        }

        /// <summary>
        /// действие по тапу на квадратик со словом
        /// </summary>
        /// <param name="wordName">слово выбранное в квадрате</param>
        private async void TapWord(string wordName)
        {
            string compareWord = !Model.isFromNative ? _showingWord.RusWord : _showingWord.EngWord;
            
            if (compareWord.Equals(wordName, StringComparison.OrdinalIgnoreCase))
            {
                SetRightMark(wordName, Color.LightBlue);
                await Task.Delay(500);
            }
            else
            {
                SetRightMark(compareWord, Color.LightBlue);
                SetRightMark(wordName, Color.Red);               
                Model.wordsOpen.Add(_showingWord);
                Model.AllOpenedWordsCount++;
                await Task.Delay(1800);
            }
           
            ShowNextWord();
            ClearBackgroundColor();
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
 
        private string _currentShowingWord;
        public string CurrentShowingWord { get => _currentShowingWord; set { _currentShowingWord = value; OnPropertyChanged(nameof(CurrentShowingWord)); } }
        private RepeatingWordsModel _model;
        public RepeatingWordsModel Model { get => _model; set { _model = value; OnPropertyChanged(nameof(Model)); } }
        private string _firstWord;
        public string FirstWord { get => _firstWord; set { _firstWord = value; OnPropertyChanged(nameof(FirstWord)); } }
        private string _secondWord;
        public string SecondWord { get => _secondWord; set { _secondWord = value; OnPropertyChanged(nameof(SecondWord)); } }
        private string _thirdWord;
        public string ThirdWord { get => _thirdWord; set { _thirdWord = value; OnPropertyChanged(nameof(ThirdWord)); } }
        private string _forthWord;
        public string ForthWord { get => _forthWord; set { _forthWord = value; OnPropertyChanged(nameof(ForthWord)); } }

        int _indexWordShowNow = -1;
        internal void ShowNextWord(bool isFirstShowAfterLoad = false)
        {
            if (_indexWordShowNow < Model.wordsCollection.Count() - 1 && _indexWordShowNow >= 0 || isFirstShowAfterLoad)
            {
                _indexWordShowNow++;
                Model.currentWord = Model.wordsCollection.ElementAt(_indexWordShowNow);
                SetViewWords(Model.currentWord, Model.isFromNative);
                Model.AllShowedWordsCount++;
                Model.wordsCollectionLeft.Remove(Model.currentWord);
            }
            else
                Debugger.Break();
        }

        private Words _showingWord;
        private void SetViewWords(Words word, bool isNative)
        {
            _showingWord = word;
            if (isNative)
                CurrentShowingWord = word.RusWord;
            else
                CurrentShowingWord = word.EngWord;
            SetSelectingWords(word, isNative);
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
                int border = Model.wordsCollection.Count() - 1;
                if (border < 3)
                    return listWordsForGrid;
                int idCurrent = Model.wordsCollection.IndexOf(word);
                int id1 = 0, id2 = 0, id3 = 0;
                while (idCurrent == id1 || idCurrent == id2 || idCurrent == id3 || id1 == id2 || id1==id3 || id3==id2)
                {
                    id1 = random.Next(0, border);
                    id2 = random.Next(0, border);
                    id3 = random.Next(0, border);
                }
                listWordsForGrid.Add(Model.wordsCollection.ElementAt(id1));
                listWordsForGrid.Add(Model.wordsCollection.ElementAt(id2));
                listWordsForGrid.Add(Model.wordsCollection.ElementAt(id3));
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

       
    }
}