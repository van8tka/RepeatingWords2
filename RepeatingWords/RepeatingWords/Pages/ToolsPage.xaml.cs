
using System;
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
                    SwLight.IsToggled = true;
                    this.BackgroundColor = Color.Silver;
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
        private async void ClickedHomeCustomButton(object sender, EventArgs e)
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
                SwLight.IsToggled = false;
                App.Current.Properties.Remove(Them);
                App.Current.Properties.Add(Them, _blackThem);
                Application.Current.Resources["TitleApp"] = Application.Current.Resources["TitleAppBlack"];
                Application.Current.Resources["LableHeadApp"] = Application.Current.Resources["LableHeadAppWhite"];
                Application.Current.Resources["LabelColor"] = Application.Current.Resources["LabelYellow"];
                Application.Current.Resources["LabelColorWB"] = Application.Current.Resources["LabelWhite"];
                Application.Current.Resources["ColorWB"] = Application.Current.Resources["ColorWhite"];
                Application.Current.Resources["ColorBlGr"] = Application.Current.Resources["ColorGreen"];
                this.BackgroundColor = Color.Black;
            }
           else
            {//при изменении IsToggled происходит вызов события switcher_ToggledLight              
                SwLight.IsToggled = true;
                App.Current.Properties.Remove(Them);
                App.Current.Properties.Add(Them, _whiteThem);
                Application.Current.Resources["TitleApp"] = Application.Current.Resources["TitleAppWhite"];
                Application.Current.Resources["LableHeadApp"] = Application.Current.Resources["LableHeadAppBlack"];
                Application.Current.Resources["LabelColor"] = Application.Current.Resources["LabelNavy"];
                Application.Current.Resources["LabelColorWB"] = Application.Current.Resources["LabelBlack"];
                Application.Current.Resources["ColorWB"] = Application.Current.Resources["ColorBlack"];
                Application.Current.Resources["ColorBlGr"] = Application.Current.Resources["ColorBlue"];
                this.BackgroundColor = Color.Silver;
            }

        }

        private void switcher_ToggledLight(object sender, ToggledEventArgs e)
        {
            //delete properties and then create new
            if (SwLight.IsToggled == true)
            {
                SwDark.IsToggled = false;             
            }
            else
            {
                SwDark.IsToggled = true;
            }
          
        }


        #region BACKUP HANDLE
        string filePathDb = App.Db.DBConnection.DatabasePath;
        string fileNameBackupDef = "backupcardsofwords";
        const string folderNameBackUp = "CardsOfWordsBackup";
        //пока только для ведроида
        //создание backup DB

        private async void BackUpButtonCkick(object sender, EventArgs e)
        {
            try
            {
                const string localFolder = "Создание резервной копии в памяти телефона";
                const string googleDriveFolder = "Создание резервной копии на Google диск";
                //создание имени файла резервной копии
                string fileNameBackup = string.Format(fileNameBackupDef + DateTime.Now.ToString("ddMMyyyy") + ".dat");

                var action = await DisplayActionSheet("Выберите способ создания резервной копии", "Отмена", null, localFolder, googleDriveFolder);
                switch (action)
                {
                    case localFolder:
                        {
                            CreateBackUpIntoDefaultFolder(fileNameBackup);
                            break;
                        }
                    case googleDriveFolder:
                        {
                            CreateBackUpIntoGoogleDrive(fileNameBackup);
                            break;
                        }
                    default: break;
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
                string filePathDefault = DependencyService.Get<IFileWorker>().CreateFolder(folderNameBackUp, fileNameBackup);
                //создаем резервную копию передаем путь к БД и путь для сохранения резервной копиии
                bool succes = DependencyService.Get<IFileWorker>().WriteFile(filePathDb, filePathDefault);
                if (succes)
                {
                    await DisplayAlert("Успешно", "Резервная копия создана в дирректории " + folderNameBackUp, "Ок");
                }
                else
                {
                    await DisplayAlert("Ошибка", "К сожалению во время создания резервной копии произошла ошибка, попробуйте другой способ создания резервной копии.", "Ок");
                }
            }
            catch (Exception er)
            {
                ErrorHandlerCustom.getErrorMessage(er);
            }
        }







        private void CreateBackUpIntoGoogleDrive(string fileNameBackup)
        {

        }










        //восстановление из backup
        private async void RestoreFromBackUpButtonCkick(object sender, EventArgs e)
        {
            try
            {
                const string localFolder = "Поиск резервной копии в памяти телефона";
                const string googleDriveFolder = "Поиск резервной копии на Google диск";
                //создание имени файла резервной копии
                string fileNameBackup = string.Format(fileNameBackupDef + DateTime.Now.ToString("ddMMyyyy") + ".dat");
                bool succes = false;
                var action = await DisplayActionSheet("Поиск резервной копии", "Отмена", null, localFolder, googleDriveFolder);
                switch (action)
                {
                    case localFolder:
                        {
                            //для восстановления данных по умолчанию
                            //получим последний файл бэкапа
                            string fileBackUp = await DependencyService.Get<IFileWorker>().GetBackUpFilesAsync(folderNameBackUp);

                            if (!string.IsNullOrEmpty(fileBackUp))
                            {
                                succes = DependencyService.Get<IFileWorker>().WriteFile(fileBackUp, filePathDb);
                            }
                            break;
                        }
                    case googleDriveFolder:
                        {

                            break;
                        }
                    default: break;
                }
                if (succes)
                {
                    await DisplayAlert("", "Резервная копия восстановлена.", "Ок");
                }
                else
                {
                    await DisplayAlert("", "К сожалению, резервная копия не обнаружена.", "Ок");
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
