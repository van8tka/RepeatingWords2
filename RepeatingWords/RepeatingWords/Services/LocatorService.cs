﻿using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Repositories;
using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.ViewModel;
 
using Unity;
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
            _container.RegisterInstance(typeof(IUnitOfWork), new UnitOfWork(sqlitePath.GetDatabasePath(Constants.DATABASE_NAME)));
            _container.RegisterType<IInitDefaultDb, InitDefaultDb>();
            _container.RegisterInstance(typeof(INavigationService), new NavigationService());
            _container.RegisterType<IDialogService, DialogService>();
            _container.RegisterType<IBackupService, BackupGoogleService>();
            _container.RegisterType<IBackupService, BackupLocalService>();
            _container.RegisterInstance(typeof(IThemeService), new ThemeChangeService());
            _container.RegisterInstance(typeof(IKeyboardTranscriptionService), new KeyboardTranscriptionChangeService());
      

            //register viewmodels
            _container.RegisterType(typeof(MainViewModel));
            _container.RegisterType(typeof(HelperViewModel));
            _container.RegisterType(typeof(InstructionAddOneWordViewModel));
            _container.RegisterType(typeof(InstructionImportFromFileViewModel));
            _container.RegisterType(typeof(SettingsViewModel));
          

            
            Container = _container;
            return _container;
        }
    }
}
