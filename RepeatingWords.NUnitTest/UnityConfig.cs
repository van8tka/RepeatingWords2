using System;
using System.Threading;
using System.Threading.Tasks;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Repositories;
using RepeatingWords.Helpers;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Service;
using RepeatingWords.Services;
using RepeatingWords.ViewModel;
using Unity;


namespace RepeatingWords.NUnitTest
{
    internal class UnityConfig
    {
        internal static UnityContainer Load()
        {
            var container = new UnityContainer();
            var testPath = new TestSQLitePath();
            container.RegisterInstance(typeof(ISQLite), testPath);
            container.RegisterType<ILogger, TestLogger>();
            container.RegisterType<ILoggerService, Log>();
            container.Resolve<ILoggerService>();
            container.RegisterType<ILoggerService, Log>();
            container.RegisterInstance(typeof(IUnitOfWork), new UnitOfWork(testPath));
            container.RegisterType<IInitDefaultDb, InitDefaultDb>();

            container.RegisterType<INavigationService, TestNavigationService>();
            container.RegisterType<IDialogService, TestDialogService>();
            container.RegisterType<IDictionaryNameLearningCreator, DictionaryNameLearningCreator>();
            container.RegisterType<IDictionaryTypeByName, DictionaryTypeByName>();
            container.RegisterType<IUnlearningWordsService, UnlerningWordsService>();
            container.RegisterType<IVolumeLanguageService, TestVolumeService>();
            container.RegisterType<IContinueWordsService, ContinueWordsService>();

            container.RegisterType<ViewModelBase, RepeatingWordsViewModel>();
            return container;
        }
    }


    // INavigationService navigationServcie, IDialogService dialogService, IUnitOfWork unitOfWork, IVolumeLanguageService volumeService, IDictionaryNameLearningCreator dictionaryNameCreator, IUnlearningWordsService unlearningWordsManager

    internal class TestVolumeService : IVolumeLanguageService
    {
        public string GetSysAbbreviationVolumeLanguage()
        {
            return "en-En";
        }

        public string GetVolumeLanguage()
        {
            throw new NotImplementedException();
        }

        public bool SetVolumeLanguage(string languageName)
        {
            throw new NotImplementedException();
        }
    }

    internal class TestDialogService : IDialogService
    {
        public void HideLoadDialog()
        {
            throw new NotImplementedException();
        }

        public Task<string> ShowActionSheetAsync(string title, string cancel, string destructive, CancellationToken token = default, params string[] buttons)
        {
            throw new NotImplementedException();
        }

        public Task ShowAlertDialog(string message, string oktext, string title = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShowConfirmAsync(string message, string title, string buttonOk, string buttonCancel)
        {
            return Task.FromResult<bool>(true);
        }

        public Task<string> ShowInputTextDialog(string message, string title = null, string okText = null, string cancelText = null, string placeholder = "", CancellationToken cancelToken = default)
        {
            throw new NotImplementedException();
        }

        public void ShowLoadDialog(string loadmsg = null)
        {
            throw new NotImplementedException();
        }
    }

    internal class TestNavigationService : INavigationService
    {
        public ViewModelBase PreviousPageViewModel => throw new NotImplementedException();

        public Task InitializeAsync()
        {
            return null ;
        }

        public Task NavigateToAsync<T>() where T : ViewModelBase
        {
            throw new NotImplementedException();
        }

        public Task NavigateToAsync<T>(object param) where T : ViewModelBase
        {
            throw new NotImplementedException();
        }

        public Task RemoveBackStackAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveLastFromBackStackAsync()
        {
            throw new NotImplementedException();
        }
    }



    internal class TestLogger : ILogger
    {
        public void Debug(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Error(string message, params object[] args)
        {
             Console.WriteLine(message, args);
        }

        public void Error(Exception e, string message, params object[] args)
        {
            Console.WriteLine(e);
            Console.WriteLine(message);
        }

        public void Error(Exception e)
        {
            Console.WriteLine(e);
        }

        public void Fatal(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Info(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Trace(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Warn(string message, params object[] args)
        {
            Console.WriteLine(message);
        }
    }

    internal class TestSQLitePath : ISQLite
    {
        //для тестирования sqlite in memory
        public string GetDatabasePath(string filename)
        {
            return ":memory:";         
        }
    }
}
