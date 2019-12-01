using RepeatingWords.DataService.Model;
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
            AllLearnedWordsCount = 0;
            AllOpenedWordsCount = 0;
            WordsOpen = new List<Words>();
            WordsLeft = new List<Words>();
            IndexWordShowNow = -1;
        }

        private int _allWordsCount;
        public int AllWordsCount { get => _allWordsCount; set { _allWordsCount = value; OnPropertyChanged(nameof(AllWordsCount)); } }
        private int _allShowedWordsCount;
        public int AllShowedWordsCount { get => _allShowedWordsCount; set { _allShowedWordsCount = value; OnPropertyChanged(nameof(AllShowedWordsCount)); } }
        private int _allLearnedWordsCount;
        public int AllLearnedWordsCount { get => _allLearnedWordsCount; set { _allLearnedWordsCount = value; OnPropertyChanged(nameof(AllLearnedWordsCount)); } }
        private int _allOpenedWordsCount;
        public int AllOpenedWordsCount { get => _allOpenedWordsCount; set { _allOpenedWordsCount = value; OnPropertyChanged(nameof(AllOpenedWordsCount)); } }

        internal bool IsOpenCurrentWord { get; set; }
        internal Words CurrentWord { get; set; }
        internal bool IsFromNative { get; set; }
        public IList<Words> WordsOpen { get; set; }
        public IList<Words> WordsLearningAll { get; set; }
        public IList<Words> WordsLeft { get; set; }
        public Dictionary Dictionary { get; internal set; }
        internal int IndexWordShowNow;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
