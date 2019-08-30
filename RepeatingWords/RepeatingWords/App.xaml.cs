using Xamarin.Forms;
using Unity;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Helpers.Interfaces;
using System.Threading.Tasks;
using RepeatingWords.Services;
using System.Diagnostics;

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
            if (Device.RuntimePlatform == Device.UWP)
               InitNavigation();        
        }

      
        private Task InitNavigation()
        {
            _container.Resolve<IInitDefaultDb>().LoadDefaultData(); 
            _container.Resolve<INewVersionAppChecker>().CheckNewVersionApp();
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