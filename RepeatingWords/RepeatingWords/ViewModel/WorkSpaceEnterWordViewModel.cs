using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RepeatingWords.ViewModel
{
    public class WorkSpaceEnterWordViewModel : INotifyPropertyChanged, ICustomContentViewModel
    {
        public WorkSpaceEnterWordViewModel()
        {
            CurrentShowingWord = "Test word";
        }

        private RepeatingWordsModel _model;
        public RepeatingWordsModel Model { get => _model; set { _model = value; OnPropertyChanged(nameof(Model)); } }

        private string _currentShowingWord;
        public string CurrentShowingWord { get => _currentShowingWord; set {
                _currentShowingWord = value; OnPropertyChanged(nameof(CurrentShowingWord)); }   }

        private string _enterAnswerWord;
        public string EnterAnswerWord
        {
            get => _enterAnswerWord; set
            {
                _enterAnswerWord = value; OnPropertyChanged(nameof(EnterAnswerWord));
            }
        }

public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
