using System.Linq;
using Xamarin.Forms;
using SimpleInjector;
using RepeatingWords.Interfaces;
using RepeatingWords.Helpers.Interfaces;
using System.Threading.Tasks;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
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
            if (Device.RuntimePlatform == Device.UWP)
               InitNavigation();
        }

      
        private Task InitNavigation()
        {
            _container.GetInstance<IThemeService>().GetCurrentTheme();
            var navService = _container.GetInstance<INavigationService>(); 
            navService.InitializeAsync();
            return Task.Run(() =>
            {
                _container.GetInstance<INewVersionAppChecker>().CheckNewVersionApp();
            });
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