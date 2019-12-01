using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        //ctor
        protected ViewModelBase(INavigationService navigationServcie, IDialogService dialogService)
        {
            DialogService = dialogService;
            NavigationService = navigationServcie;
        }

        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;

       // public ICommand GoMainPageCommand { get; set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(nameof(IsBusy)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName=null)
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
