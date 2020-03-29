using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Repositories;
using RepeatingWords.Helpers;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.ViewModel;
using SimpleInjector;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    internal class LocatorService
    {

        public static Container Container { get; private set; }

        internal static Container Boot(ISQLite sqlitePath)
        {
            var _container = new Container();
            _container.RegisterInstance(typeof(ISQLite), sqlitePath);     
            _container.RegisterInstance(typeof(ILoggerService),new Log(DependencyService.Get<ILogManager>().GetLog()));
            var unitOfWork = new UnitOfWork(sqlitePath);
            var initDb = new InitDefaultDb(unitOfWork);
            _container.RegisterInstance(typeof(IUnitOfWork), unitOfWork);
            _container.RegisterInstance(typeof(INavigationService), new NavigationService());
            _container.RegisterInstance(typeof(IDialogService), new DialogService());          
            _container.Register<BackupGoogleService>();
            _container.Register<BackupLocalService>();
            _container.Register<IThemeService, ThemeChangeService>();
            _container.Register<IShowLanguage, ShowLanguageService>();
            _container.Register<IKeyboardTranscriptionService, KeyboardTranscriptionChangeService>();
            _container.Register<IWebClient, WebClient>();
            _container.Register<IImportFile, ImportFileToDb>();
            _container.Register<IVolumeLanguageService, VolumeLanguageService>();
             _container.Register<INewVersionAppChecker, NewVersionAppChecker>();
            _container.Register<ILanguageLoaderFacade, LanguageLoader>();
            _container.Register<IAnimationService, AnimationService>();
            _container.Register<IEntryWordValidator, EntryWordValidator>();
            _container.Register<ITextToSpeech, SpeechService>();
            _container.RegisterInstance(typeof(IStudyService), new StudyService(unitOfWork, initDb));
      
            //register viewmodels
            _container.Register(typeof(MainViewModel));
            _container.Register(typeof(HelperViewModel));
            _container.Register(typeof(InstructionAddOneWordViewModel));
            _container.Register(typeof(InstructionImportFromFileViewModel));
            _container.Register(typeof(SettingsViewModel));
            _container.Register(typeof(LanguageFrNetViewModel));
            _container.Register(typeof(WordsListViewModel));
            _container.Register(typeof(RepeatingWordsViewModel));
            _container.Register(typeof(CreateWordViewModel));
            _container.Register(typeof(EntryTranscriptionViewModel));
            //must befor RepeatingWords
            _container.Register(typeof(WorkSpaceCardsViewModel));
            _container.Register(typeof(WorkSpaceEnterWordViewModel));
            _container.Register(typeof(WorkSpaceSelectWordViewModel));                     
            _container.Register(typeof(VolumeLanguagesViewModel));

            Container = _container;
            return _container;
        }
    }
}
