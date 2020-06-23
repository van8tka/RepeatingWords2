﻿using System.Linq;
using Xamarin.Forms;
using SimpleInjector;
using RepeatingWords.Interfaces;
using RepeatingWords.Helpers.Interfaces;
using System.Threading.Tasks;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Xamarin.Forms.Xaml;
using RepeatingWords.LoggerService;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RepeatingWords
{
    public partial class App : Application
    {
           //ctor   
        public App(ISQLite sqlitePath, Log log)
        {           
            InitializeComponent();
            _container = LocatorService.Boot(sqlitePath, log);
        }

        private Container _container;
        
      
        private void InitNavigation()
        {
            _container.GetInstance<IThemeService>().GetCurrentTheme();
            var navService = _container.GetInstance<INavigationService>(); 
            navService.InitializeAsync();
            _container.GetInstance<INewVersionAppChecker>().CheckNewVersionApp();
        }

        protected override void OnStart()
        {
            InitNavigation();
        }
        protected override void OnSleep()
        { }
        protected override void OnResume()
        { }       
    }
}