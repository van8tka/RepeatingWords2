using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Repositories;
using RepeatingWords.Heleprs;
using RepeatingWords.Helpers;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.ViewModel;
 
using Unity;
using Unity.Lifetime;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    internal class LocatorService
    {

        public static IUnityContainer Container { get; private set; }

        internal static IUnityContainer Boot(ISQLite sqlitePath)
        {
            var _container = new UnityContainer();
            _container.RegisterInstance(typeof(ISQLite), sqlitePath);
            _container.RegisterInstance(typeof(ILogger), DependencyService.Get<ILogManager>().GetLog());
            _container.RegisterType<ILoggerService, Log>();
            _container.Resolve<ILoggerService>();
            _container.RegisterType<IUnitOfWork, UnitOfWork>(new ExternallyControlledLifetimeManager());
            _container.RegisterType<IInitDefaultDb, InitDefaultDb>();
            _container.RegisterInstance(typeof(INavigationService), new NavigationService());
            _container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IBackupService, BackupGoogleService>();
            _container.RegisterType<IBackupService, BackupLocalService>();
            _container.RegisterType<IThemeService, ThemeChangeService>();
            _container.RegisterType<IKeyboardTranscriptionService, KeyboardTranscriptionChangeService>();
            _container.RegisterType<IWebApiService, OnlineDictionaryService>();
            _container.RegisterType<IImportFile, ImportFileToDb>();
            _container.RegisterType<IVolumeLanguageService, VolumeLanguageService>();
            _container.RegisterType<IDictionaryNameLearningCreator, DictionaryNameLearningCreator>();
            _container.RegisterType<IUnlearningWordsManager, UnlerningWordsManager>();
            _container.RegisterType<INewVersionAppChecker, NewVersionAppChecker>();
            _container.RegisterType<ILanguageLoaderFacade, LanguageLoader>();
            _container.RegisterType<IContinueWordsManager, ContinueWordsManager>();
            
            //register viewmodels
            _container.RegisterType(typeof(MainViewModel));
            _container.RegisterType(typeof(HelperViewModel));
            _container.RegisterType(typeof(InstructionAddOneWordViewModel));
            _container.RegisterType(typeof(InstructionImportFromFileViewModel));
            _container.RegisterType(typeof(SettingsViewModel));
            _container.RegisterType(typeof(DictionariesListViewModel));
            _container.RegisterType(typeof(LanguageFrNetViewModel));
            _container.RegisterType(typeof(WordsListViewModel));
            _container.RegisterType(typeof(RepeatingWordsViewModel));
            _container.RegisterType(typeof(CreateWordViewModel));
            _container.RegisterType(typeof(EntryTranscriptionViewModel));
            _container.RegisterType(typeof(ChooseFileViewModel));
            //must befor RepeatingWords
            _container.RegisterType(typeof(WorkSpaceCardsViewModel));
            _container.RegisterType(typeof(WorkSpaceEnterWordViewModel));
            _container.RegisterType(typeof(WorkSpaceSelectWordViewModel));
            
            _container.RegisterType(typeof(RepeatingWordsViewModel));
            _container.RegisterType(typeof(VolumeLanguagesViewModel));
            Container = _container;
            return _container;
        }
    }
}
