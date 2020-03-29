using System.ComponentModel;
using System.Runtime.CompilerServices;
using RepeatingWords.Annotations;

namespace RepeatingWords.Model
{
  public class BaseModel:INotifyPropertyChanged
    {

        private int _id;
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
