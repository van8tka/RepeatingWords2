using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.Helpers.Interfaces;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        //ctor
        protected ViewModelBase(INavigationService navigationServcie, IDialogService dialogService)
        {
            DialogService = dialogService;
            NavigationService = navigationServcie;
            GoMainPageCommand = new Command(async () => {
                await NavigationService.NavigateToAsync<MainViewModel>();
                await NavigationService.RemoveBackStackAsync(); });
        }
                

        //todo dialog service
        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;

        public ICommand GoMainPageCommand { get; set; }

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
