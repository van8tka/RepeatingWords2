using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RepeatingWords.ViewModel
{
    public class WorkSpaceSelectWordViewModel : INotifyPropertyChanged, ICustomContentViewModel
    {
        public WorkSpaceSelectWordViewModel()
        {

        }
        private RepeatingWordsModel _model;
        public RepeatingWordsModel Model { get => _model; set { _model = value; OnPropertyChanged(nameof(Model)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
