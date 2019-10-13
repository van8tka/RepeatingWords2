using Xamarin.Forms;
using SimpleInjector;
using RepeatingWords.Interfaces;
using RepeatingWords.Helpers.Interfaces;
using System.Threading.Tasks;
using RepeatingWords.Services;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
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

        private Container _container;
        private void InitApp(ISQLite sqlitePath)
        {           
            _container = LocatorService.Boot(sqlitePath);
            var init = _container.GetInstance<IInitDefaultDb>();
            Task.Run(() => init.LoadDefaultData());
            if (Device.RuntimePlatform == Device.UWP)
               InitNavigation();        
        }

      
        private Task InitNavigation()
        {
            Task.Run(() =>
            {
                _container.GetInstance<INewVersionAppChecker>().CheckNewVersionApp();
            });
            _container.GetInstance<IThemeService>().GetCurrentTheme();
                           
            var navService = _container.GetInstance<INavigationService>();
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