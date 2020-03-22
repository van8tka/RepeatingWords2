using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.View;
using RepeatingWords.ViewModel;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using FormsControls.Base;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class NavigationService : INavigationService
    {
        private AnimationNavigationPage _mainPage => Application.Current.MainPage as AnimationNavigationPage;
        public ViewModelBase PreviousPageViewModel
        {
            get => _mainPage.Navigation.NavigationStack[_mainPage.Navigation.NavigationStack.Count - 2].BindingContext as ViewModelBase;
        }

        public Task GoBackPage()
        {
          return _mainPage.Navigation.PopAsync();
        }
        

        public Task InitializeAsync()
        {
            return NavigateToAsync<MainViewModel>();
        }

        public Task NavigateToAsync<T>() where T : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(T), null);
        }

        public Task NavigateToAsync<T>(object param) where T : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(T), param);
        }

        public Task RemoveLastFromBackStackAsync()
        {
            
            if(_mainPage!=null)
            {
                _mainPage.Navigation.RemovePage(_mainPage.Navigation.NavigationStack[_mainPage.Navigation.NavigationStack.Count - 2]);
            }
            return Task.FromResult(true);
        }

        public Task RemoveBackStackAsync()
        {
            try
            {              
                if (_mainPage != null)
                {                  
                    while( _mainPage.Navigation.NavigationStack.Count - 1 > 0)
                    {
                        Page page = _mainPage.Navigation.NavigationStack[0];
                        _mainPage.Navigation.RemovePage(page);
                    }
                }
                return Task.FromResult(true);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }           
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            var page = CreatePage(viewModelType, parameter);
            if(_mainPage != null)
            {
                await _mainPage.PushAsync(page);
            }
            else
            {
                Application.Current.MainPage = new AnimationNavigationPage(page)
                {
                    BarBackgroundColor = Color.FromHex("#6bafef"),
                    BarTextColor = Color.White
                };
            }
            await (page.BindingContext as ViewModelBase)?.InitializeAsync(parameter);
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            var pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
                throw new Exception($"Can't locate page type to {viewModelType}");
            return (Activator.CreateInstance(pageType) as Page);
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0},{1}", viewName, viewModelAssemblyName);
            return Type.GetType(viewAssemblyName);
        }
    }
}
