using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
using Java.IO;

namespace RepeatingWords.Droid
{

    [Activity(Label = "Cards of words", MainLauncher = true, Theme = "@style/MyTheme.Splash", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //для установки SplashScreen обязательно использовать FormsAppCompatActivity а не FormsApplicationActivity
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity,GoogleApiClient.IConnectionCallbacks, IResultCallback, IDriveApiDriveContentsResult
    {

       

        public static bool HasPermissionToReadWriteExternalStorage = false;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
             //проверка наличия разрешения
                CheckPermissionForStorage();          
            LoadApplication(new App());
             
        }
        //переопределение кнопки назад
        public async override void OnBackPressed()
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
                if(ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage)==(int)Permission.Granted && ContextCompat.CheckSelfPermission(this,Manifest.Permission.WriteExternalStorage)==(int)Permission.Granted)
                {
                    HasPermissionToReadWriteExternalStorage = true;
                }
                else
                {
                    var requestPermission = new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage };
                    ActivityCompat.RequestPermissions(this, requestPermission, REQUEST_STORAGE);
                }
            }
            catch(Exception er)
            { }
        }
        //обработка результата установки разрешения
        public override void OnRequestPermissionsResult(int requestCode,string[] permissions, Permission[] grantResults)
        {
            try
            {
                if(requestCode==REQUEST_STORAGE)
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
            catch(Exception er)
            { }
        }







        ///WORK WITH GOOGLE DRIVE



        //GoogleApiClient.IConnectionCallbacks, IResultCallback, IDriveApiDriveContentsResult
        const string TAG = "GDriveExample";
        const int REQUEST_CODE_RESOLUTION = 3;
        GoogleApiClient _googleApiClient;
        //папка с бэкапом                
        string folderName = "CardsOfWordsBackUp";
        string filename = "cardsofwordsbackup";
        //флаг создаем файл бэкапа или получаем файл бэкапа
        bool isCreateBackUp = true;




        //авторизация гугл 
        public void GoogleCustomAuthorithation(bool isCreateBackUp)
        {
            this.isCreateBackUp = isCreateBackUp;
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
            { }
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
                        // Log.Error(TAG, "Unable to sign in, is app registered for Drive access in Google Dev Console?");
                        break;
                    case Result.FirstUser:
                        /// Log.Error(TAG, "Unable to sign in: RESULT_FIRST_USER");
                        break;
                    default:
                        //  Log.Error(TAG, "Should never be here: " + resultCode);
                        return;
                }
            }
        }
        //если удачно законектились 
        void IResultCallback.OnResult(Java.Lang.Object result)
        {
            var contentResults = (result).JavaCast<IDriveApiDriveContentsResult>();
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




        string fileLocal = "fileDbOnSmart.txt";

        //получаем файл бэкапа
        private async void GetBackUpFile(IDriveApiDriveContentsResult contentResults)
        {
            var contentFile = new StringBuilder();
            //получаем папку бэкапа            
            DriveId folderBackUpId = FindItems(folderName).Result;
            if (folderBackUpId == null)
            {
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
                }
                //чтение файла и создание нового 
                var readFile = await driveFile.OpenAsync(_googleApiClient, DriveFile.ModeReadOnly, null);
                using (var inpstr = readFile.DriveContents.InputStream)
                using (var streamReade = new StreamReader(inpstr))
                {
                    while (streamReade.Peek() >= 0)
                    {
                        contentFile.Append(await streamReade.ReadLineAsync());
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine(contentFile.ToString());
        }






        //создаем бэкап
        private void CreateBackUpFolderAndFile(IDriveApiDriveContentsResult contentResults)
        {
            string filenameCreate = filename + DateTime.Now.ToString("ddMMyyyy") + ".dat";
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
            WriteFile(folderBackUpId, filenameCreate, contentResults);
        }




        //поиск папки для бэкапа
        private async Task<DriveId> FindItems(string folderName)
        {
            DriveId folderId = null;
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





        //создаем папку в гугле
        private void CreateFolder(string folderName)
        {
            MetadataChangeSet changeSet = new MetadataChangeSet.Builder()
                        .SetTitle(folderName)
                        .SetMimeType(DriveFolder.MimeType)
                        .SetStarred(true)
                        .Build();
            var result = DriveClass.DriveApi.GetRootFolder(_googleApiClient).CreateFolder(_googleApiClient, changeSet);

        }


        //метод записи файла
        private void WriteFile(DriveId folderBackUpId, string filename, IDriveApiDriveContentsResult content)
        {
            var writer = new OutputStreamWriter(content.DriveContents.OutputStream);
            writer.Write("backup string in base 64 string");
            writer.Close();
            MetadataChangeSet changeSet = new MetadataChangeSet.Builder()
                   .SetTitle(filename)
                   .SetMimeType("application/octet-stream")
                   .Build();
            //DriveClass.DriveApi
            //          .GetRootFolder(_googleApiClient)
            //          .CreateFile(_googleApiClient, changeSet, content.DriveContents);
            IDriveFolder driveFolder = null;
            //получаем папку по ID
            driveFolder = driveFolder ?? folderBackUpId.AsDriveFolder();
            //если папка не ноль то создаем файл
            if (driveFolder != null)
                driveFolder.CreateFile(_googleApiClient, changeSet, content.DriveContents);
        }






        public void OnConnectionSuspended(int cause)
        {
            throw new NotImplementedException();
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

