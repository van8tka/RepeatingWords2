using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Unity;

namespace RepeatingWords.ViewModel
{
    public class SettingsViewModel:ViewModelBase
    {
        //ctor
        public SettingsViewModel(INavigationService navigationService, IDialogService dialogService, IThemeService themeService, IKeyboardTranscriptionService transcriptKeyboardService):base(navigationService, dialogService)
        {
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
            _transcriptKeyboardService = transcriptKeyboardService ?? throw new ArgumentNullException(nameof(transcriptKeyboardService));
            SwitchThemeCommand = new Command(SwitchThemeApp);
            SwitchTranskriptionKeyboardCommand = new Command(SwitchTranscriptionKeyboard);
            BackUpCommand = new Command(async () => { await ChoseCreateBackUp(); }); ;
            RestoreBackUpCommand = new Command(async() => { await RestoreBackup(); });
            SetCurrentSettings();
        }

        private void SetCurrentSettings()
        {
            IsCustomKeyboardTranscription = _transcriptKeyboardService.GetCurrentTranscriptionKeyboard();
            IsDarkThem = _themeService.GetCurrentTheme();
        }

        private readonly IThemeService _themeService;
        private readonly IKeyboardTranscriptionService _transcriptKeyboardService;
        private string _fileNameBackupDef = "backupcardsofwords";
        private string _localFolderBackup = Resource.BackUpInLocal;
        private string _googleDriveFolderBackup = Resource.BackUpOnGoogle;

        private bool _isCustomTranscriptionKeyboard;
        public bool IsCustomKeyboardTranscription { get=> _isCustomTranscriptionKeyboard; set { _isCustomTranscriptionKeyboard = value; OnPropertyChanged(nameof(IsCustomKeyboardTranscription)); } }

        private bool _isDarkThem;
        public bool IsDarkThem { get => _isDarkThem; set { _isDarkThem = value; OnPropertyChanged(nameof(_isDarkThem)); } }


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
            catch(Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            return base.InitializeAsync(navigationData);
        }

      
        //переменная отображения стиля темы
        private const string Them = "theme";
        private const string _whiteThem = "white";
        private const string _blackThem = "black";
        //переменные для показа клавиатуры транскрипции
        private const string TrKeyboard = "TrKeyboard";
        private const string showKeyboard = "true";
        private const string UnShowKeyboard = "false";


        public ICommand SwitchThemeCommand { get; set; }
        public ICommand SwitchTranskriptionKeyboardCommand { get; set; }
        public ICommand BackUpCommand { get; set; }
        public ICommand RestoreBackUpCommand { get; set; }



        private async Task ChoseCreateBackUp()
        {
            try
            {                            
                //создание имени файла резервной копии
                string fileNameBackup = string.Format(_fileNameBackupDef + DateTime.Now.ToString("ddMMyyyy_hhmm") + ".dat");
                var action = await DialogService.ShowActionSheetAsync(Resource.BackupMethod, Resource.ModalActCancel, null, default , _localFolderBackup, _googleDriveFolderBackup);
                bool success ;
                if (action == _localFolderBackup)
                {
                    var backupService = LocatorService.Container.Resolve<BackupLocalService>();
                    success  = await backupService.CreateBackup(fileNameBackup);
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
            var action = await  DialogService.ShowActionSheetAsync( Resource.BackUpSearch, Resource.ModalActCancel, null, default, _localFolderBackup, _googleDriveFolderBackup);
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


        //private void switcher_ToggledShowKeyboard(object sender, ToggledEventArgs e)
        //{
        //    if (SwShowKeyboard.IsToggled == true)
        //    {
        //        App.Current.Properties.Remove(TrKeyboard);
        //        App.Current.Properties.Add(TrKeyboard, showKeyboard);
        //    }
        //    else
        //    {
        //        App.Current.Properties.Remove(TrKeyboard);
        //        App.Current.Properties.Add(TrKeyboard, UnShowKeyboard);
        //    }

        //}


        ////обработка переключателей
        //private void switcher_ToggledDark(object sender, ToggledEventArgs e)
        //{
        //    //delete properties and then create new

        //    if (SwDark.IsToggled == true)
        //    {

        //        App.Current.Properties.Remove(Them);
        //        App.Current.Properties.Add(Them, _blackThem);
        //        Application.Current.Resources["TitleApp"] = Application.Current.Resources["TitleAppBlack"];
        //        Application.Current.Resources["LableHeadApp"] = Application.Current.Resources["LableHeadAppWhite"];
        //        Application.Current.Resources["LabelColor"] = Application.Current.Resources["LabelYellow"];
        //        Application.Current.Resources["PickerColor"] = Application.Current.Resources["PickerColorYellow"];
        //        Application.Current.Resources["LabelColorWB"] = Application.Current.Resources["LabelWhite"];
        //        Application.Current.Resources["ColorWB"] = Application.Current.Resources["ColorWhite"];
        //        Application.Current.Resources["ColorBlGr"] = Application.Current.Resources["ColorYellow"];
        //        this.BackgroundColor = Color.FromHex("#363636");
        //    }
        //    else
        //    {//при изменении IsToggled происходит вызов события switcher_ToggledLight              

        //        App.Current.Properties.Remove(Them);
        //        App.Current.Properties.Add(Them, _whiteThem);
        //        Application.Current.Resources["TitleApp"] = Application.Current.Resources["TitleAppWhite"];
        //        Application.Current.Resources["LableHeadApp"] = Application.Current.Resources["LableHeadAppBlack"];
        //        Application.Current.Resources["LabelColor"] = Application.Current.Resources["LabelNavy"];
        //        Application.Current.Resources["PickerColor"] = Application.Current.Resources["PickerColorNavy"];
        //        Application.Current.Resources["LabelColorWB"] = Application.Current.Resources["LabelBlack"];
        //        Application.Current.Resources["ColorWB"] = Application.Current.Resources["ColorBlack"];
        //        Application.Current.Resources["ColorBlGr"] = Application.Current.Resources["ColorBlue"];
        //        this.BackgroundColor = Color.FromHex("#f5f5f5");
        //    }

        //}






    }
}
