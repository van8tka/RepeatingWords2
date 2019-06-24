using System.ComponentModel;
using System.Runtime.CompilerServices;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.ViewModel
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {

        //todo dialog service
        //protected readonly IDialogService DialogService;
        protected readonly INavigationServcie NavigationService;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanges([CallerMemberName] string PropertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
