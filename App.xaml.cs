using System;
using Xamarin.Forms;
using Unity;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Helpers.Interfaces;
using System.Threading.Tasks;
using RepeatingWords.Services;

namespace RepeatingWords
{
    public partial class App : Application
    {
           //ctor   
        public App(ISQLite sqlitePath)
        {            
            InitializeComponent();
            _container = LocatorService.Boot(sqlitePath);
            InitApp(sqlitePath);                           
        }

        private readonly IUnityContainer _container;
        private void InitApp(ISQLite sqlitePath)
        {
            Log.Logger.Info("Init default settings app");
            if (Device.RuntimePlatform == Device.UWP)
                InitNavigation();
            _container.Resolve<IThemeService>().GetCurrentTheme();
            _container.Resolve<IKeyboardTranscriptionService>().GetCurrentTranscriptionKeyboard();          
            InitDb();
        }

        private Task InitNavigation()
        {
           var navService = _container.Resolve<INavigationService>();
           return navService.InitializeAsync();
        }

        private void InitDb()
        {
            var init = _container.Resolve<IInitDefaultDb>();
            init.LoadDefaultData();
        }

        protected override async void OnStart()
        {
            if (Device.RuntimePlatform != Device.UWP)
                await InitNavigation();

        }
        protected override void OnSleep()
        { }
        protected override void OnResume()
        { }       
    }
}