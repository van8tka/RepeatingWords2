using RepeatingWords.DataService.Interfaces;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        //ctor
        public SettingsViewModel(INavigationService navigationService, IDialogService dialogService, IThemeService themeService, IKeyboardTranscriptionService transcriptKeyboardService, IVolumeLanguageService volumeService, IFirstLanguage firstLanguageService) : base(navigationService, dialogService)
        {
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
            _transcriptKeyboardService = transcriptKeyboardService ?? throw new ArgumentNullException(nameof(transcriptKeyboardService));
            _volumeService = volumeService ?? throw new ArgumentNullException(nameof(transcriptKeyboardService));
            _firstLanguageService = firstLanguageService ?? throw new ArgumentNullException(nameof(firstLanguageService));
            SetCurrentSettings();
            SwitchThemeCommand = new Command(SwitchThemeApp);
            SwitchTranskriptionKeyboardCommand = new Command(SwitchTranscriptionKeyboard);
            SwitchFirstLanguageCommand = new Command(SwitchFirstLanguageShow);
            BackUpCommand = new Command(async () => { await ChooseCreateBackUp(); }); ;
            RestoreBackUpCommand = new Command(async () => { await RestoreBackup(); });
            ChangeVoiceLanguageCommand = new Command(async () => await NavigationService.NavigateToAsync<VolumeLanguagesViewModel>(this));           
        }

        private readonly IThemeService _themeService;
        private readonly IKeyboardTranscriptionService _transcriptKeyboardService;
        private readonly IVolumeLanguageService _volumeService;
        private readonly IFirstLanguage _firstLanguageService;
        private string _fileNameBackupDef = "backupcardsofwords";
        private string _localFolderBackup = Resource.BackUpInLocal;
        private string _googleDriveFolderBackup = Resource.BackUpOnGoogle;

        private bool _isCustomTranscriptionKeyboard;
        public bool IsCustomKeyboardTranscription { get => _isCustomTranscriptionKeyboard; set { _isCustomTranscriptionKeyboard = value; OnPropertyChanged(nameof(IsCustomKeyboardTranscription)); } }

        private bool _isDarkThem;
        public bool IsDarkThem { get => _isDarkThem; set { _isDarkThem = value; OnPropertyChanged(nameof(IsDarkThem)); } }

        private bool _currentFirstLanguageNative;
        public bool CurrentFirstLanguageNative { get => _currentFirstLanguageNative; set { _currentFirstLanguageNative = value; OnPropertyChanged(nameof(CurrentFirstLanguageNative)); } }

        private string _currentVoiceLanguage; 
        public string CurrentVoiceLanguage { get => _currentVoiceLanguage; set { _currentVoiceLanguage = value; OnPropertyChanged(nameof(CurrentVoiceLanguage)); } }

        public ICommand SwitchThemeCommand { get; set; }
        public ICommand SwitchTranskriptionKeyboardCommand { get; set; }
        public ICommand BackUpCommand { get; set; }
        public ICommand RestoreBackUpCommand { get; set; }
        public ICommand ChangeVoiceLanguageCommand { get; set; }
        public ICommand SwitchFirstLanguageCommand { get; set; }



        private void SwitchTranscriptionKeyboard()
        {
            try
            {              
                    Log.Logger.Info("Change theme of app");
                    _transcriptKeyboardService.ChangeUsingTranscriptionKeyboard();                               
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private void SwitchThemeApp()
        {
            try
            {             
                    Log.Logger.Info("Change theme of app");
                    _themeService.ChangeTheme();               
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private void SwitchFirstLanguageShow()
        {
            try
            {
                Log.Logger.Info("Change first language");
                _firstLanguageService.ChangeFirstLanguage();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }
        

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            return base.InitializeAsync(navigationData);
        }
      
        private void SetCurrentSettings()
        {           
                IsCustomKeyboardTranscription = _transcriptKeyboardService.GetCurrentTranscriptionKeyboard();
                IsDarkThem = _themeService.GetCurrentTheme();
                CurrentVoiceLanguage = _volumeService.GetVolumeLanguage().Name;
                CurrentFirstLanguageNative = _firstLanguageService.GetFirstLanguage();
        }

        private async Task ChooseCreateBackUp()
        {
            try
            {
                //создание имени файла резервной копии
                string fileNameBackup = string.Format(_fileNameBackupDef + DateTime.Now.ToString("ddMMyyyy_hhmm") + ".dat");
                var action = await DialogService.ShowActionSheetAsync(Resource.BackupMethod,"", Resource.ModalActCancel, default, _localFolderBackup, _googleDriveFolderBackup);
                bool success;
                if (action == _localFolderBackup)
                {
                    var backupService = LocatorService.Container.GetInstance<BackupLocalService>();
                    success = await backupService.CreateBackup(fileNameBackup);
                     DialogService.ShowToast(success ? Resource.BackupWasCreatedGoogle : Resource.BackUpErrorCreated);
                }
                else if (action == _googleDriveFolderBackup)
                {
                    var backupService = LocatorService.Container.GetInstance<BackupGoogleService>();
                    success = await backupService.CreateBackup(fileNameBackup);
                }
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
            }
        }


        private async Task RestoreBackup()
        {
            var action = await DialogService.ShowActionSheetAsync(Resource.BackUpSearch, "", Resource.ModalActCancel, default, _localFolderBackup, _googleDriveFolderBackup);
            bool success = false;
            if (action == _localFolderBackup)
            {
                var backupService = LocatorService.Container.GetInstance<BackupLocalService>();
                success = await backupService.RestoreBackup(_fileNameBackupDef);
                DialogService.ShowToast(success ? Resource.BackupRestored : Resource.BackUpErrorCreated);
            }
            else if (action == _googleDriveFolderBackup)
            {
                var backupService = LocatorService.Container.GetInstance<BackupGoogleService>();
                success = await backupService.RestoreBackup(_fileNameBackupDef);             
            }           
        }
    }
}
