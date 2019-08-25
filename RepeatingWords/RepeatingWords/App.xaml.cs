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
        private async Task InitApp(ISQLite sqlitePath)
        {
            Log.Logger.Info("Init default settings app");          
            if (Device.RuntimePlatform == Device.UWP)
               await InitNavigation();
            await _container.Resolve<INewVersionAppChecker>().CheckNewVersionApp();
            InitDb();
            _container.Resolve<IThemeService>().GetCurrentTheme();           
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