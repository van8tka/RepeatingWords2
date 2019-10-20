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
            var init = _container.GetInstance<IInitDefaultDb>();
            Task.Run(() => init.LoadDefaultData());
            if (Device.RuntimePlatform == Device.UWP)
               InitNavigation();
        }

      
        private Task InitNavigation()
        {
            _container.GetInstance<IThemeService>().GetCurrentTheme();
            var navService = _container.GetInstance<INavigationService>(); 
            navService.InitializeAsync(); 
            ContinueLearning(navService);
            return Task.Run(() =>
            {
                _container.GetInstance<INewVersionAppChecker>().CheckNewVersionApp();
            });
        }
        /// <summary>
        /// if exist dictionary which started learned and didn't finish, will be loading continue learned
        /// </summary>
        /// <param name="navService"></param>
        /// <returns></returns>
        private void ContinueLearning(INavigationService navService)
        {
            var unitOfWork = _container.GetInstance<IUnitOfWork>();
            var lastAction = unitOfWork.LastActionRepository.Get().LastOrDefault();
            if(lastAction!=null)
                 navService.NavigateToAsync<RepeatingWordsViewModel>(lastAction);
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