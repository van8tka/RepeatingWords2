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
        public SettingsViewModel(INavigationService navigationService, IDialogService dialogService, IThemeService themeService, IKeyboardTranscriptionService transcriptKeyboardService, IVolumeLanguageService volumeService, IShowLanguage showLanguageService) : base(navigationService, dialogService)
        {
            _themeService = themeService;
            _transcriptKeyboardService = transcriptKeyboardService;
            _volumeService = volumeService;
            _showLanguageService = showLanguageService;
            SetCurrentSettings();
            SwitchThemeCommand = new Command(SwitchThemeApp);
            SwitchTranskriptionKeyboardCommand = new Command(SwitchTranscriptionKeyboard);
            ChangeFirstLanguageCommand = new Command(SwitchFirstLanguageShow);
            BackUpCommand = new Command(async () => { await ChooseCreateBackUp(); }); ;
            RestoreBackUpCommand = new Command(async () => { await RestoreBackup(); });
            ChangeVoiceLanguageCommand = new Command(async () => await NavigationService.NavigateToAsync<VolumeLanguagesViewModel>(this));
        }

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true; 
            CurrentVoiceLanguage = _volumeService.GetVolumeLanguage(); 
            SetFirstLanguageView(_showLanguageService.GetFirstLanguage()); 
            return base.InitializeAsync(navigationData);
        }

        private readonly IThemeService _themeService;
        private readonly IKeyboardTranscriptionService _transcriptKeyboardService;
        private readonly IVolumeLanguageService _volumeService;
        private readonly IShowLanguage _showLanguageService;
        private string _fileNameBackupDef = "backupcardsofwords";
        private string _localFolderBackup = Resource.BackUpInLocal;
        private string _googleDriveFolderBackup = Resource.BackUpOnGoogle;

        private bool _isCustomTranscriptionKeyboard;
        public bool IsCustomKeyboardTranscription { get => _isCustomTranscriptionKeyboard; set { _isCustomTranscriptionKeyboard = value; OnPropertyChanged(nameof(IsCustomKeyboardTranscription)); } }

        private bool _isDarkThem;
        public bool IsDarkThem { get => _isDarkThem; set { _isDarkThem = value; OnPropertyChanged(nameof(IsDarkThem)); } }

        private string _currentLanguageView;
        public string CurrentLanguageView { get => _currentLanguageView; set { _currentLanguageView = value; OnPropertyChanged(nameof(CurrentLanguageView)); } }

        private string _currentVoiceLanguage; 
        public string CurrentVoiceLanguage { get => _currentVoiceLanguage; set { _currentVoiceLanguage = value; OnPropertyChanged(nameof(CurrentVoiceLanguage)); } }

        public ICommand SwitchThemeCommand { get; set; }
        public ICommand SwitchTranskriptionKeyboardCommand { get; set; }
        public ICommand BackUpCommand { get; set; }
        public ICommand RestoreBackUpCommand { get; set; }
        public ICommand ChangeVoiceLanguageCommand { get; set; }
        public ICommand ChangeFirstLanguageCommand { get; set; }

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
                SetFirstLanguageView(_showLanguageService.ChangeFirstLanguage());
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }
        
        private void SetCurrentSettings()
        {           
            IsCustomKeyboardTranscription = _transcriptKeyboardService.GetCurrentTranscriptionKeyboard();
            IsDarkThem = _themeService.GetCurrentTheme();
        }

        private void SetFirstLanguageView(bool isNativeFirst)
        {
            CurrentLanguageView = isNativeFirst ? Resource.LabelFirstLanguageNative:Resource.LabelFirstLanguageForeign;
        }

        private async Task ChooseCreateBackUp()
        {
            try
            {
                //создание имени файла резервной копии
                string fileNameBackup = string.Format(_fileNameBackupDef + DateTime.Now.ToString("ddMMyyyy_hhmm") + ".json");
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
                success = await backupService.RestoreBackup(string.Empty);
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
