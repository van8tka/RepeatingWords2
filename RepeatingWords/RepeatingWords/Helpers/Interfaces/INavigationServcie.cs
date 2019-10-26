using RepeatingWords.ViewModel;
using System.Threading.Tasks;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface INavigationService
    {
        ViewModelBase PreviousPageViewModel { get; }
        Task InitializeAsync();
        Task NavigateToAsync<T>() where T : ViewModelBase;
        Task NavigateToAsync<T>(object param) where T : ViewModelBase;
        Task RemoveLastFromBackStackAsync();
        Task RemoveBackStackAsync();
        Task GoBackPage();
    }
}
