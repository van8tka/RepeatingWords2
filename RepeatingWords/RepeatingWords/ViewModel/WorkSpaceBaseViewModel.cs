using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RepeatingWords.ViewModel
{
    public abstract class WorkSpaceBaseViewModel: INotifyPropertyChanged, ICustomContentViewModel
    {
        private string _currentShowingWord;
        public string CurrentShowingWord { get => _currentShowingWord; set { _currentShowingWord = value; OnPropertyChanged(nameof(CurrentShowingWord)); } }
        private RepeatingWordsModel _model;
        public RepeatingWordsModel Model { get => _model; set { _model = value; OnPropertyChanged(nameof(Model)); } }


        protected int _indexWordShowNow = -1;
        internal void ShowNextWord(bool isFirstShowAfterLoad = false)
        {
            if(isFirstShowAfterLoad)
                _indexWordShowNow = -1;
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
       

        internal abstract void SetViewWords(Words currentWord, bool isFromNative);

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
