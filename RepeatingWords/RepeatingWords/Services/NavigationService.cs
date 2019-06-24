using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    internal class NavigationService : INavigationServcie
    {

        private IReadOnlyList<Page> _navigationStack => Application.Current.MainPage.Navigation.NavigationStack;

        public ViewModelBase PreviousPageViewModel
        {
            get => _navigationStack[_navigationStack.Count - 2].BindingContext as ViewModelBase;
        }

        public Task InitializeAsync()
        {
            return NavigateToAsync<MainPageVM>();
        }

        public Task NavigateToAsync<T>() where T : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(T), null);
        }

        public Task NavigateToAsync<T>(object param) where T : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(T), param);
        }

        public Task RemoveBackStackAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveLastFromBackStackAsync()
        {
            throw new NotImplementedException();
        }

        private Task InternalNavigateToAsync(Type type, object p)
        {
            throw new NotImplementedException();
        }
    }
}
