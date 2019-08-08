using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        //ctor
        public SettingsViewModel(INavigationService navigationService, IDialogService dialogService, IThemeService themeService, IKeyboardTranscriptionService transcriptKeyboardService, IVolumeLanguageService volumeService) : base(navigationService, dialogService)
        {
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
            _transcriptKeyboardService = transcriptKeyboardService ?? throw new ArgumentNullException(nameof(transcriptKeyboardService));
            _volumeService = volumeService ?? throw new ArgumentNullException(nameof(transcriptKeyboardService));
            SetCurrentSettings();
            SwitchThemeCommand = new Command(SwitchThemeApp);
            SwitchTranskriptionKeyboardCommand = new Command(SwitchTranscriptionKeyboard);
            BackUpCommand = new Command(async () => { await ChoseCreateBackUp(); }); ;
            RestoreBackUpCommand = new Command(async () => { await RestoreBackup(); });
            ChangeVoiceLanguageCommand = new Command(async () => await NavigationService.NavigateToAsync<VolumeLanguagesViewModel>());
        }

        private readonly IThemeService _themeService;
        private readonly IKeyboardTranscriptionService _transcriptKeyboardService;
        private readonly IVolumeLanguageService _volumeService;
        private string _fileNameBackupDef = "backupcardsofwords";
        private string _localFolderBackup = Resource.BackUpInLocal;
        private string _googleDriveFolderBackup = Resource.BackUpOnGoogle;

        private bool _isCustomTranscriptionKeyboard;
        public bool IsCustomKeyboardTranscription { get => _isCustomTranscriptionKeyboard; set { _isCustomTranscriptionKeyboard = value; OnPropertyChanged(nameof(IsCustomKeyboardTranscription)); } }

        private bool _isDarkThem;
        public bool IsDarkThem { get => _isDarkThem; set { _isDarkThem = value; OnPropertyChanged(nameof(IsDarkThem)); } }

        private string _currentVoiceLanguage; 
        public string CurrentVoiceLanguage { get => _currentVoiceLanguage; set { _currentVoiceLanguage = value; OnPropertyChanged(nameof(CurrentVoiceLanguage)); } }

        public ICommand SwitchThemeCommand { get; set; }
        public ICommand SwitchTranskriptionKeyboardCommand { get; set; }
        public ICommand BackUpCommand { get; set; }
        public ICommand RestoreBackUpCommand { get; set; }
        public ICommand ChangeVoiceLanguageCommand { get; set; }



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

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;                  
            return base.InitializeAsync(navigationData);
        }
      
        private void SetCurrentSettings()
        {           
            IsCustomKeyboardTranscription = _transcriptKeyboardService.GetCurrentTranscriptionKeyboard();
            IsDarkThem = _themeService.GetCurrentTheme();
            CurrentVoiceLanguage = _volumeService.GetVolumeLanguage();
        }

        private async Task ChoseCreateBackUp()
        {
            try
            {
                //создание имени файла резервной копии
                string fileNameBackup = string.Format(_fileNameBackupDef + DateTime.Now.ToString("ddMMyyyy_hhmm") + ".dat");
                var action = await DialogService.ShowActionSheetAsync(Resource.BackupMethod, Resource.ModalActCancel, null, default, _localFolderBackup, _googleDriveFolderBackup);
                bool success;
                if (action == _localFolderBackup)
                {
                    var backupService = LocatorService.Container.Resolve<BackupLocalService>();
                    success = await backupService.CreateBackup(fileNameBackup);
                    await DialogService.ShowAlertDialog(success ? Resource.BackupWasCreatedGoogle : Resource.BackUpErrorCreated, Resource.Continue);
                }
                else if (action == _googleDriveFolderBackup)
                {
                    var backupService = LocatorService.Container.Resolve<BackupGoogleService>();
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
            var action = await DialogService.ShowActionSheetAsync(Resource.BackUpSearch, Resource.ModalActCancel, null, default, _localFolderBackup, _googleDriveFolderBackup);
            bool success;
            if (action == _localFolderBackup)
            {
                var backupService = LocatorService.Container.Resolve<BackupLocalService>();
                success = await backupService.RestoreBackup(_fileNameBackupDef);
                await DialogService.ShowAlertDialog(success ? Resource.BackupWasCreatedGoogle : Resource.BackUpErrorCreated, Resource.Continue);
            }
            else if (action == _googleDriveFolderBackup)
            {
                var backupService = LocatorService.Container.Resolve<BackupGoogleService>();
                success = await backupService.RestoreBackup(_fileNameBackupDef);
            }
        }




        //string localFolder = Resource.BackUpInLocal;
        //string googleDriveFolder = Resource.BackUpOnGoogle;
        //string titleSearchBackUp = Resource.BackUpSearch;
        ////восстановление из backup
        //private async void RestoreFromBackUpButtonCkick(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        
        //        bool succes = false;
        //        var action = await DisplayActionSheet(titleSearchBackUp, Resource.ModalActCancel, null, localFolder, googleDriveFolder);

        //        if (action == localFolder)
        //        {
        //            //для восстановления данных по умолчанию
        //            //получим последний файл бэкапа
        //            string fileBackUp = await DependencyService.Get<IFileWorker>().GetBackUpFilesAsync(folderNameBackUp);

        //            if (!string.IsNullOrEmpty(fileBackUp))
        //            {
        //                succes = DependencyService.Get<IFileWorker>().WriteFile(fileBackUp, filePathToDbFull);
        //                if (succes)
        //                    await DisplayAlert("", successRestore, "Ок");
        //                else
        //                    await DisplayAlert("", ErrorRestore, "Ок");
        //            }
        //        }
        //        else if (action == googleDriveFolder)
        //        {
        //            succes = DependencyService.Get<IGoogleDriveWorker>().RestoreBackupGoogleDriveFile(filePathToDbFull, fileNameBackupDef, folderNameBackUp, successRestore, ErrorRestore);
        //        }
        //    }
        //    catch (Exception er)
        //    {
        //        Log.Logger.Error(er);
        //    }
        //}





        //public SettingsView()
        //{
        //    InitializeComponent();

        //    //fullscreen advertizing
        //    DependencyService.Get<IAdmobInterstitial>().Show("ca-app-pub-5993977371632312/4024752876");

        //    object propThem = "";
        //    object propTrKeyb = "";
        //    if (App.Current.Properties.TryGetValue(Them, out propThem))
        //    {
        //        if (propThem.Equals(_blackThem))
        //        {
        //            SwDark.IsToggled = true;
        //        }
        //        else
        //        {
        //            SwDark.IsToggled = false;
        //            this.BackgroundColor = Color.White;
        //        }
        //    }
        //    if (App.Current.Properties.TryGetValue(TrKeyboard, out propTrKeyb))
        //    {
        //        if (propTrKeyb.Equals(showKeyboard))
        //        {
        //            SwShowKeyboard.IsToggled = true;
        //        }
        //        else
        //        {
        //            SwShowKeyboard.IsToggled = false;
        //        }
        //    }
        //}

    }
}
