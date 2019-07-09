using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Drive;
using Android.Gms.Drive.Query;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Java.IO;
using Xamarin.Forms;

namespace RepeatingWords.Droid
{

    [Activity(Label = "Cards of words", MainLauncher = true, Theme = "@style/MyTheme.Splash", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //для установки SplashScreen обязательно использовать FormsAppCompatActivity а не FormsApplicationActivity
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, GoogleApiClient.IConnectionCallbacks, IResultCallback, IDriveApiDriveContentsResult
    {

        internal static MainActivity Instance { get; private set; }
     

        public static bool HasPermissionToReadWriteExternalStorage = false;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Instance = this;
            Android.Gms.Ads.MobileAds.Initialize(ApplicationContext, "ca-app-pub-5993977371632312~7094124560");
            global::Xamarin.Forms.Forms.Init(this, bundle);
            //проверка наличия разрешения
            CheckPermissionForStorage();                     
            LoadApplication(new App( new SQLite_Android()));
            UserDialogs.Init(() => (Activity)Forms.Context);
        }
        //переопределение кнопки назад
        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }
        //код получения разрешения
        const int REQUEST_STORAGE = 1010;
        //проверка наличия разрешения на доступ к памяти смартфона
        private void CheckPermissionForStorage()
        {
            try
            {
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted && ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted)
                {
                    HasPermissionToReadWriteExternalStorage = true;
                }
                else
                {
                    var requestPermission = new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage };
                    ActivityCompat.RequestPermissions(this, requestPermission, REQUEST_STORAGE);
                }
            }
            catch (Exception er)
            {
                Log.Error("CheckPermissionForStorage", er.Message);
            }
        }
        //обработка результата установки разрешения
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                if (requestCode == REQUEST_STORAGE)
                {
                    if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
                        HasPermissionToReadWriteExternalStorage = true;
                    else
                        HasPermissionToReadWriteExternalStorage = false;
                }
                else
                {
                    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                    HasPermissionToReadWriteExternalStorage = false;
                }
            }
            catch (Exception er)
            {
                Log.Error("OnRequestPermissionsResult", er.Message);
            }
        }







        ///WORK WITH GOOGLE DRIVE



        //GoogleApiClient.IConnectionCallbacks, IResultCallback, IDriveApiDriveContentsResult
        //    const string TAG = "CardsOfWords";
        const string TAG = "clientcardsofwordsandroid";
        const int REQUEST_CODE_RESOLUTION = 3;
        GoogleApiClient _googleApiClient;
        //папка с бэкапом                
        string folderName = string.Empty;
        string filename = string.Empty;
        string pathToDb = string.Empty;
        //флаг создаем файл бэкапа или получаем файл бэкапа
        bool isCreateBackUp = true;

       

        string successMessage;
        string errorMessage;

        //авторизация гугл 
        public void GoogleCustomAuthorithation(bool isCreateBackUp, string folderName = null, string fileName = null, string pathToDb = null, string successMessage = "Excelent", string errorMessage = "Error")
        {
            this.isCreateBackUp = isCreateBackUp;
            this.folderName = folderName;
            filename = fileName;
            this.pathToDb = pathToDb;
            this.successMessage = successMessage;
            this.errorMessage = errorMessage;
            //создаем клиент гугл
            if (_googleApiClient == null)
            {
                _googleApiClient = new GoogleApiClient.Builder(this)
                  .AddApi(DriveClass.API)
                  .AddScope(DriveClass.ScopeFile)
                  .AddConnectionCallbacks(this)
                  .AddOnConnectionFailedListener(onConnectionFailed)
                  .Build();
            }
            if (!_googleApiClient.IsConnected)
                _googleApiClient.Connect();
            else
            {
                DoWorkBackupOrRestore(contentResults);
            }
        }




        //если ошибка подключения
        private void onConnectionFailed(ConnectionResult result)
        {
            if (!result.HasResolution)
            {
                GoogleApiAvailability.Instance.GetErrorDialog(this, result.ErrorCode, 0).Show();
                return;
            }
            try
            {
                result.StartResolutionForResult(this, REQUEST_CODE_RESOLUTION);
            }
            catch (IntentSender.SendIntentException e)
            {
                CreateAlertDialog("", errorMessage+e.Message);
            }
        }


        //подключаемся к гугл диску
        public void OnConnected(Bundle connectionHint)
        {
            DriveClass.DriveApi.NewDriveContents(_googleApiClient).SetResultCallback(this);
        }


        //
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == REQUEST_CODE_RESOLUTION)
            {
                switch (resultCode)
                {
                    case Result.Ok:
                        _googleApiClient.Connect();
                        break;
                    case Result.Canceled:
                        CreateAlertDialog("", errorMessage);                     
                        break;
                    case Result.FirstUser:
                        CreateAlertDialog("", errorMessage);                     
                        break;
                    default:
                        CreateAlertDialog("", errorMessage);                      
                        return;
                }
            }
        }


        IDriveApiDriveContentsResult contentResults;

        //если удачно законектились 
        void IResultCallback.OnResult(Java.Lang.Object result)
        {
            contentResults = (result).JavaCast<IDriveApiDriveContentsResult>();
            DoWorkBackupOrRestore(contentResults);
        }

        private void DoWorkBackupOrRestore(IDriveApiDriveContentsResult contentResults)
        {
            if(contentResults!=null)
            {
                if (!contentResults.Status.IsSuccess) // handle the error
                    return;
                Task.Run(() =>
                {
                    if (isCreateBackUp)
                        CreateBackUpFolderAndFile(contentResults);
                    else
                        GetBackUpFile(contentResults);
                });
            }
            else
                CreateAlertDialog("", errorMessage);

        }




      

        //получаем файл бэкапа
        private async void GetBackUpFile(IDriveApiDriveContentsResult contentResults)
        {
            try
            {
                var contentFile = new StringBuilder();
                //получаем папку бэкапа            
                DriveId folderBackUpId = FindItems(folderName).Result;
                if (folderBackUpId == null)
                {
                    CreateAlertDialog("", errorMessage);
                    //папка с бэкапом не обнаружена
                }
                else
                {
                    IDriveFolder driveFolder = null;
                    IDriveFile driveFile = null;
                    //получаем папку по ID
                    driveFolder = driveFolder ?? folderBackUpId.AsDriveFolder();
                    //если папка не ноль то получаем файлы                
                    if (driveFolder != null)
                    {
                        var filesResult = await driveFolder.ListChildrenAsync(_googleApiClient);
                        Java.Util.Date temp = null;
                        foreach (var item in filesResult.MetadataBuffer)
                        {
                            if (item.Title.Contains(filename))
                            {
                                if (temp == null || !temp.After(item.ModifiedDate))
                                {
                                    temp = item.ModifiedDate;
                                    driveFile = item.DriveId.AsDriveFile();
                                }
                            }
                        }

                        //чтение файла из google drive и получение строки в BAse64       

                        var readFile = await driveFile.OpenAsync(_googleApiClient, DriveFile.ModeReadOnly, null);
                        using (var inpstr = readFile.DriveContents.InputStream)
                        using (var streamReade = new StreamReader(inpstr))
                        {
                            while (streamReade.Peek() >= 0)
                            {
                                contentFile.Append(await streamReade.ReadLineAsync());
                            }
                        }
                        //конвернтируем строку из base64 и записываем в файл БД(переписываем)
                        byte[] bytes = Convert.FromBase64String(contentFile.ToString());
                        System.IO.File.WriteAllBytes(pathToDb, bytes);
                        CreateAlertDialog("", successMessage);

                    }
                   
                }
                
            }
            catch(Exception er)
            {
                Log.Error(er.Message, er.StackTrace);
                CreateAlertDialog("", errorMessage);
            }                  
        }
       
        private void CreateAlertDialog(string title, string message)
        {
            RunOnUiThread(() =>
            {
                AlertDialog.Builder builder;
                builder = new AlertDialog.Builder(this);
                builder.SetTitle(title);
                builder.SetMessage(message);
                builder.SetCancelable(false);
                builder.SetPositiveButton("OK", delegate { });
                builder.Show();
            });
        }





        //создаем бэкап
        private void CreateBackUpFolderAndFile(IDriveApiDriveContentsResult contentResults)
        {
            try
            {
                // string filenameCreate = filename + DateTime.Now.ToString("ddMMyyyy") + ".dat";
                //поиск папки с бэкапом
                DriveId folderBackUpId = FindItems(folderName).Result;
                //если папка не найдена создаем папку в гугле диске
                if (folderBackUpId == null)
                {
                    CreateFolder(folderName);
                    //получаем ID 
                    folderBackUpId = FindItems(folderName).Result;
                }
                //записываем файл в папку
                WriteFile(folderBackUpId, filename, contentResults);
            }
            catch(Exception er)
            {
                Log.Error("ERROR", er.Message + " "+er.StackTrace);
            }
        
        }




        //поиск папки для бэкапа
        private async Task<DriveId> FindItems(string folderName)
        {
            DriveId folderId = null;
            try
            {              
                //установим запрос поиска
                IDriveFolder appFolder = DriveClass.DriveApi.GetRootFolder(_googleApiClient);
                var query = new QueryClass
                  .Builder().AddFilter(Filters.And(
                  Filters.Eq(SearchableField.Title, folderName),
                  Filters.Eq(SearchableField.Trashed, false))).Build();
                var queryResult = await appFolder.QueryChildrenAsync(_googleApiClient, query);
                foreach (var driveItem in queryResult.MetadataBuffer)
                {
                    if (driveItem.IsFolder && driveItem.Title == folderName)
                    {
                        folderId = driveItem.DriveId;
                    }
                }
                return folderId;
            }
            catch (Exception er)
            {
                Log.Error("ERROR", er.Message + " " + er.StackTrace);
                return folderId;
            }
        }





        //создаем папку в гугле
        private void CreateFolder(string folderName)
        {
            try
            {
                MetadataChangeSet changeSet = new MetadataChangeSet.Builder()
                                       .SetTitle(folderName)
                                       .SetMimeType(DriveFolder.MimeType)
                                       .SetStarred(true)
                                       .Build();
                var result = DriveClass.DriveApi.GetRootFolder(_googleApiClient).CreateFolder(_googleApiClient, changeSet);
            }
            catch (Exception er)
            {
                Log.Error("ERROR", er.Message + " " + er.StackTrace);
            }
        }


        //метод записи файла
        private void WriteFile(DriveId folderBackUpId, string filename, IDriveApiDriveContentsResult content)
        {
            try
            {
                byte[] bytes = System.IO.File.ReadAllBytes(pathToDb);
                string file = Convert.ToBase64String(bytes);
                using (var writer = new OutputStreamWriter(content.DriveContents.OutputStream))
                {
                    writer.Write(file);
                    writer.Close();
                }
                MetadataChangeSet changeSet = new MetadataChangeSet.Builder()
                       .SetTitle(filename)
                       .SetMimeType("application/octet-stream")
                       .Build();
                IDriveFolder driveFolder = null;
                //получаем папку по ID
                driveFolder = driveFolder ?? folderBackUpId.AsDriveFolder();
                //если папка не ноль то создаем файл
                if (driveFolder != null)
                {
                   var s = driveFolder.CreateFile(_googleApiClient, changeSet, content.DriveContents);
                   CreateAlertDialog("", successMessage);
                }
                   
            }
            catch(Exception er)
            {
                CreateAlertDialog("", errorMessage+er.Message);
            }          
        }






        public void OnConnectionSuspended(int cause)
        {
            CreateAlertDialog("", errorMessage);
            Log.Error("ERRor", "OnConnectionSuspended");
        }

        public IDriveContents DriveContents
        {
            get
            {
               throw new NotImplementedException();
            }
        }

        public Statuses Status
        {
            get
            {
               throw new NotImplementedException();
            }
        }


    }
}

