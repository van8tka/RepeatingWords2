using RepeatingWords.DataService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RepeatingWords.Model
{
    public class RepeatingWordsModel : INotifyPropertyChanged
    {

        public RepeatingWordsModel()
        {
            AllWordsCount = 0;
            AllShowedWordsCount = 0;
            AllOpenedWordsCount = 0;
            wordsOpen = new List<Words>();
            wordsCollectionLeft = new List<Words>();
        }

        private int _allWordsCount;
        public int AllWordsCount { get => _allWordsCount; set { _allWordsCount = value; OnPropertyChanged(nameof(AllWordsCount)); } }
        private int _allShowedWordsCount;
        public int AllShowedWordsCount { get => _allShowedWordsCount; set { _allShowedWordsCount = value; OnPropertyChanged(nameof(AllShowedWordsCount)); } }
        private int _allOpenedWordsCount;
        public int AllOpenedWordsCount { get => _allOpenedWordsCount; set { _allOpenedWordsCount = value; OnPropertyChanged(nameof(AllOpenedWordsCount)); } }

        internal Words currentWord { get; set; }
        internal bool isFromNative { get; set; }
        public IList<Words> wordsOpen { get; set; }
        public IEnumerable<Words> wordsCollection { get; set; }
        public IList<Words> wordsCollectionLeft { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public void ResetModel()
        {
            AllShowedWordsCount = 1;
            AllOpenedWordsCount = 0;
            wordsOpen.Clear();
            (wordsCollectionLeft as List<Words>).AddRange(wordsCollection);
        }
    }
}
