using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        //ctor
        protected ViewModelBase(INavigationServcie navigationServcie, IDialogService dialogService)
        {
            DialogService = dialogService;
            NavigationService = navigationServcie;
        }
                

        //todo dialog service
        protected readonly IDialogService DialogService;
        protected readonly INavigationServcie NavigationService;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanges(nameof(IsBusy)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanges([CallerMemberName] string PropertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// передача данных во VM при навигации
        /// </summary>    
        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
