
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.Pages
{

    public partial class ToolsPage : ContentPage
    {


        //переменная отображения стиля темы
        const string Them = "theme";
        const string _whiteThem = "white";
        const string _blackThem = "black";
        //переменные для показа клавиатуры транскрипции
        const string TrKeyboard = "TrKeyboard";
        const string showKeyboard = "true";
        const string UnShowKeyboard = "false";

        public ToolsPage()
        {
            InitializeComponent();

            //fullscreen advertizing
            DependencyService.Get<IAdmobInterstitial>().Show("ca-app-pub-5993977371632312/4024752876");
    
            object propThem = "";
            object propTrKeyb = "";
            if (App.Current.Properties.TryGetValue(Them, out propThem))
            {
                if (propThem.Equals(_blackThem))
                {                  
                    SwDark.IsToggled = true;
                }
                else
                {
                    SwDark.IsToggled = false;
                    this.BackgroundColor = Color.White;
                }
            }
            if(App.Current.Properties.TryGetValue(TrKeyboard, out propTrKeyb))
            {
                if (propTrKeyb.Equals(showKeyboard))
                {
                   SwShowKeyboard.IsToggled = true;
                }
                else
                {
                    SwShowKeyboard.IsToggled = false;
                }
            }
        }



        //вызов главной страницы и чистка стека страниц
        private void ClickedHomeCustomButton(object sender, EventArgs e)
        {
            //выход на главную страницу
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        private void switcher_ToggledShowKeyboard(object sender, ToggledEventArgs e)
        {
          if(SwShowKeyboard.IsToggled==true)
            {
                App.Current.Properties.Remove(TrKeyboard);
                App.Current.Properties.Add(TrKeyboard, showKeyboard);
            }
          else
            {
                App.Current.Properties.Remove(TrKeyboard);
                App.Current.Properties.Add(TrKeyboard, UnShowKeyboard);
            }
          
        }


        //обработка переключателей
        private void switcher_ToggledDark(object sender, ToggledEventArgs e)
        {
            //delete properties and then create new
           
             if (SwDark.IsToggled == true)
            {               
                
                App.Current.Properties.Remove(Them);
                App.Current.Properties.Add(Them, _blackThem);
                Application.Current.Resources["TitleApp"] = Application.Current.Resources["TitleAppBlack"];
                Application.Current.Resources["LableHeadApp"] = Application.Current.Resources["LableHeadAppWhite"];
                Application.Current.Resources["LabelColor"] = Application.Current.Resources["LabelYellow"];
                Application.Current.Resources["PickerColor"] = Application.Current.Resources["PickerColorYellow"];
                Application.Current.Resources["LabelColorWB"] = Application.Current.Resources["LabelWhite"];
                Application.Current.Resources["ColorWB"] = Application.Current.Resources["ColorWhite"];
                Application.Current.Resources["ColorBlGr"] = Application.Current.Resources["ColorYellow"];
                this.BackgroundColor = Color.FromHex("#363636");
            }
           else
            {//при изменении IsToggled происходит вызов события switcher_ToggledLight              
                
                App.Current.Properties.Remove(Them);
                App.Current.Properties.Add(Them, _whiteThem);
                Application.Current.Resources["TitleApp"] = Application.Current.Resources["TitleAppWhite"];
                Application.Current.Resources["LableHeadApp"] = Application.Current.Resources["LableHeadAppBlack"];
                Application.Current.Resources["LabelColor"] = Application.Current.Resources["LabelNavy"];
                Application.Current.Resources["PickerColor"] = Application.Current.Resources["PickerColorNavy"];
                Application.Current.Resources["LabelColorWB"] = Application.Current.Resources["LabelBlack"];
                Application.Current.Resources["ColorWB"] = Application.Current.Resources["ColorBlack"];
                Application.Current.Resources["ColorBlGr"] = Application.Current.Resources["ColorBlue"];
                this.BackgroundColor = Color.FromHex("#f5f5f5");
            }

        }



        #region BACKUP HANDLE


        //   string filePathToDbFull = App.Db.DBConnection.DatabasePath;
        string filePathToDbFull {
            get
            {
                Debugger.Break();
              //  App.Db.DBConnection.DatabasePath
                return null;
            }
            set
            {
                Debugger.Break();
            }
        }
        string fileNameBackupDef = "backupcardsofwords";
        const string folderNameBackUp = "CardsOfWordsBackup";




        //пока только для ведроида
        //создание backup DB

        private async void BackUpButtonCkick(object sender, EventArgs e)
        {
            try
            {               
                string localFolder = Resource.BackUpCreateLocal; 
                 string googleDriveFolder = Resource.BackUpGoogleDrive;
                 string choseMethodToCreateBackUp = Resource.BackupMethod;
                //создание имени файла резервной копии
                string fileNameBackup = string.Format(fileNameBackupDef + DateTime.Now.ToString("ddMMyyyy") + ".dat");

                var action = await DisplayActionSheet(choseMethodToCreateBackUp, Resource.ModalActCancel, null, localFolder, googleDriveFolder);

                if(action == localFolder)
                {
                    await CreateBackUpIntoDefaultFolder(fileNameBackup);
                }
                else if(action == googleDriveFolder)
                {
                    CreateBackUpIntoGoogleDrive(fileNameBackup);
                }
                               
            }
            catch (Exception er)
            {
                ErrorHandlerCustom.getErrorMessage(er);
            }

        }

        //сохранение в папку по умолчанию
        private async Task CreateBackUpIntoDefaultFolder(string fileNameBackup)
        {
            try
            {//получим путь к папке

                 string titleSuccess = Resource.SuccessStr;
                 string backUpWasCreated = Resource.BackupWasCreatedInFolder+" ";
                string filePathDefault = DependencyService.Get<IFileWorker>().CreateFolder(folderNameBackUp, fileNameBackup);
                //создаем резервную копию передаем путь к БД и путь для сохранения резервной копиии
                bool succes = DependencyService.Get<IFileWorker>().WriteFile(filePathToDbFull, filePathDefault);
                if (succes)
                {
                    await DisplayAlert(titleSuccess, backUpWasCreated + folderNameBackUp, "Ок");
                }
                else
                {
                    await DisplayAlert(Resource.ModalException, ErrorCreateBack, "Ок");
                }
            }
            catch (Exception er)
            {
                ErrorHandlerCustom.getErrorMessage(er);
            }
        }







        private void CreateBackUpIntoGoogleDrive(string fileNameBackup)
        {
            //передаем название папки бэкапа.название файла которые нужно создать и путь к файлу БД
           DependencyService.Get<IGoogleDriveWorker>().CreateBackupGoogleDrive(folderNameBackUp, fileNameBackup, filePathToDbFull, successCreateBack, ErrorCreateBack);
        }




        string successRestore = Resource.BackupRestored;
        string successCreateBack = Resource.BackupWasCreatedGoogle;
        string ErrorRestore = Resource.BackUpErrorRestored; 
        string ErrorCreateBack = Resource.BackUpErrorCreated; 

         string localFolder = Resource.BackUpInLocal; 
        string googleDriveFolder = Resource.BackUpOnGoogle;
        string titleSearchBackUp = Resource.BackUpSearch; 
        //восстановление из backup
        private async void RestoreFromBackUpButtonCkick(object sender, EventArgs e)
        {
            try
            {
                       
                //создание имени файла резервной копии
                string fileNameBackup = string.Format(fileNameBackupDef + DateTime.Now.ToString("ddMMyyyy") + ".dat");
                bool succes = false;
                var action = await DisplayActionSheet(titleSearchBackUp, Resource.ModalActCancel, null, localFolder, googleDriveFolder);

                if(action == localFolder)
                {
                    //для восстановления данных по умолчанию
                    //получим последний файл бэкапа
                    string fileBackUp = await DependencyService.Get<IFileWorker>().GetBackUpFilesAsync(folderNameBackUp);

                    if (!string.IsNullOrEmpty(fileBackUp))
                    {
                        succes = DependencyService.Get<IFileWorker>().WriteFile(fileBackUp, filePathToDbFull);
                        if (succes)
                            await DisplayAlert("", successRestore, "Ок");
                        else
                            await DisplayAlert("", ErrorRestore, "Ок");
                    }
                }
                else if(action == googleDriveFolder)
                {
                    succes = DependencyService.Get<IGoogleDriveWorker>().RestoreBackupGoogleDriveFile(filePathToDbFull, fileNameBackupDef, folderNameBackUp, successRestore, ErrorRestore);
                }
            }
            catch (Exception er)
            {
                ErrorHandlerCustom.getErrorMessage(er);
            }
        }
            #endregion

    }

}
