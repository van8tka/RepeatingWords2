using Xamarin.Forms;
using Unity;
using RepeatingWords.Interfaces;
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
            InitApp(sqlitePath);           
        }

        private IUnityContainer _container;
        private void InitApp(ISQLite sqlitePath)
        {           
            _container = LocatorService.Boot(sqlitePath);
            var init = _container.Resolve<IInitDefaultDb>();
            Task.Run(() => init.LoadDefaultData());
            if (Device.RuntimePlatform == Device.UWP)
               InitNavigation();        
        }

      
        private Task InitNavigation()
        {
            Task.Run(() =>
            {
                _container.Resolve<INewVersionAppChecker>().CheckNewVersionApp();
            });
            _container.Resolve<IThemeService>().GetCurrentTheme();
                           
            var navService = _container.Resolve<INavigationService>();
            return navService.InitializeAsync();
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