using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.Interfaces;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        //ctor
        public SettingsViewModel(INavigationService navigationService, IDialogService dialogService,
            IThemeService themeService, IKeyboardTranscriptionService transcriptKeyboardService,
            IVolumeLanguageService volumeService, IShowLanguage showLanguageService) : base(navigationService,
            dialogService)
        {
            _themeService = themeService;
            _transcriptKeyboardService = transcriptKeyboardService;
            _volumeService = volumeService;
            _showLanguageService = showLanguageService;
            SetCurrentSettings();
            SwitchThemeCommand = new Command(SwitchThemeApp);
            SwitchTranskriptionKeyboardCommand = new Command(SwitchTranscriptionKeyboard);
            ChangeFirstLanguageCommand = new Command(SwitchFirstLanguageShow);
            BackUpCommand = new Command(async () => { await ContextMenyBackup(); });
            ;
            RestoreBackUpCommand = new Command(async () => { await RestoreBackup(); });
            ChangeVoiceLanguageCommand = new Command(async () =>
                await NavigationService.NavigateToAsync<VolumeLanguagesViewModel>(this));
            BackUpLocalCommand = new Command(async () => { await BackupLocal(); });
            BackUpGoogleCommand = new Command(async () => { await BackupGoogle(); });
            RestoreLocalCommand = new Command(async () => { await RestoreLocal(); });
            RestoreGoogleCommand = new Command(async () => { await RestoreGoogle(); });
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

        public bool IsCustomKeyboardTranscription
        {
            get => _isCustomTranscriptionKeyboard;
            set
            {
                _isCustomTranscriptionKeyboard = value;
                OnPropertyChanged(nameof(IsCustomKeyboardTranscription));
            }
        }

        private bool _isDarkThem;

        public bool IsDarkThem
        {
            get => _isDarkThem;
            set
            {
                _isDarkThem = value;
                OnPropertyChanged(nameof(IsDarkThem));
            }
        }

        private string _currentLanguageView;

        public string CurrentLanguageView
        {
            get => _currentLanguageView;
            set
            {
                _currentLanguageView = value;
                OnPropertyChanged(nameof(CurrentLanguageView));
            }
        }

        private string _currentVoiceLanguage;

        public string CurrentVoiceLanguage
        {
            get => _currentVoiceLanguage;
            set
            {
                _currentVoiceLanguage = value;
                OnPropertyChanged(nameof(CurrentVoiceLanguage));
            }
        }

        public ICommand SwitchThemeCommand { get; set; }
        public ICommand SwitchTranskriptionKeyboardCommand { get; set; }
        public ICommand BackUpCommand { get; set; }
        public ICommand BackUpLocalCommand { get; set; }
        public ICommand BackUpGoogleCommand { get; set; }
        public ICommand RestoreBackUpCommand { get; set; }
        public ICommand RestoreLocalCommand { get; set; }
        public ICommand RestoreGoogleCommand { get; set; }
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
            CurrentLanguageView =
                isNativeFirst ? Resource.LabelFirstLanguageNative : Resource.LabelFirstLanguageForeign;
        }

        private async Task ContextMenyBackup()
        {
            var action = await DialogService.ShowActionSheetAsync(Resource.BackupMethod, "", Resource.ModalActCancel,
                default, _localFolderBackup, _googleDriveFolderBackup);
            if (action == _localFolderBackup)
                BackUpLocalCommand.Execute(null);
            else if (action == _googleDriveFolderBackup)
                BackUpGoogleCommand.Execute(null);
        }

        private string GetBackupFileName =>
            string.Format(_fileNameBackupDef + DateTime.Now.ToString("ddMMyyyy_hhmm") + ".json");

        private async Task BackupLocal()
        {
            try
            {
                DialogService.ShowLoadDialog();
                var backupService = LocatorService.Container.GetInstance<BackupLocalService>();
                bool success = await backupService.CreateBackup(GetBackupFileName);
                DialogService.ShowToast(success ? Resource.BackupWasCreatedGoogle : Resource.BackUpErrorCreated);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
            finally
            {
                DialogService.HideLoadDialog();
            }
        }

        private async Task BackupGoogle()
        {
            try
            {
                DialogService.ShowLoadDialog();
                var backupService = LocatorService.Container.GetInstance<BackupGoogleService>();
                bool success = await backupService.CreateBackup(GetBackupFileName);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
            finally
            {
                DialogService.HideLoadDialog();
            }
        }


        private async Task RestoreBackup()
        {
            var action = await DialogService.ShowActionSheetAsync(Resource.BackUpSearch, "", Resource.ModalActCancel,
                default, _localFolderBackup, _googleDriveFolderBackup);
            if (action == _localFolderBackup)
                RestoreLocalCommand.Execute(null);
            else if (action == _googleDriveFolderBackup)
                RestoreGoogleCommand.Execute(null);
        }

        private async Task RestoreLocal()
        {
            try
            {
                DialogService.ShowLoadDialog();
                var backupService = LocatorService.Container.GetInstance<BackupLocalService>();
                bool success = await backupService.RestoreBackup(string.Empty);
                DialogService.ShowToast(success ? Resource.BackupRestored : Resource.BackUpErrorCreated);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
            finally
            {
                DialogService.HideLoadDialog();
            }
        }

        private async Task RestoreGoogle()
        {
            try
            {
                DialogService.ShowLoadDialog();
                var backupService = LocatorService.Container.GetInstance<BackupGoogleService>();
                bool success = await backupService.RestoreBackup(_fileNameBackupDef);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
            finally
            {
                DialogService.HideLoadDialog();
            }
        }
    }
}
