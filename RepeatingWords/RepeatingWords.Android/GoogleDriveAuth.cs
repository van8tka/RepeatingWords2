using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Drive;
using Android.Gms.Drive.Query;
using Android.OS;
using Android.Runtime;
using Java.IO;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;

namespace RepeatingWords.Droid
{
    public partial class MainActivity
    {
        ///WORK WITH GOOGLE DRIVE
        //для версии release and debug необходимо разные версии регистрации в googleApis (cardsofwordsV21  - debug, CardsOfWords - release )
        //возможно необходима регистрация OAuth2 для каждой новой версии(надо разобраться)


        const int REQUEST_CODE_RESOLUTION = 3;
        GoogleApiClient _googleApiClient;
        //папка с резервной копией                
        string folderName = string.Empty;
        string filename = string.Empty;
        string pathToDb = string.Empty;
        //флаг создаем файл резервной копии или получаем файл резервной копии
        bool isCreateBackUp = true;
        string successMessage;
        string errorMessage;
        private Func<string, Task<bool>> _restoreFunc;
        private IDialogService _dialogService;
        private IDriveApiDriveContentsResult _contentResults;
        //авторизация Google
        public void GoogleCustomAuthorithation(bool isCreateBackUp, IDialogService dialogService ,string folderName = null, string fileName = null, string pathToDb = null, string successMessage = "Excelent", string errorMessage = "Error", Func<string,Task<bool>> restoreFunc = null)
        {
            this.folderName = folderName;
            this.filename = fileName;
            this.pathToDb = pathToDb;
            this.isCreateBackUp = isCreateBackUp;
            this.successMessage = successMessage;
            this.errorMessage = errorMessage;
            this._restoreFunc = restoreFunc;
            _dialogService = dialogService;
            CreateGoogleClient();
            RunBackupOrRestore();
        }

        private void CreateGoogleClient()
        {
            if (_googleApiClient == null)
            {
                _googleApiClient = new GoogleApiClient.Builder(this)
                  .AddApi(DriveClass.API)
                  .AddScope(DriveClass.ScopeFile)
                  .AddConnectionCallbacks(this)
                  .AddOnConnectionFailedListener(onConnectionFailed)
                  .Build();
            }           
        }

        private void RunBackupOrRestore()
        {
            if (!_googleApiClient.IsConnected)
                _googleApiClient.Connect();
            else
                DoWorkBackupOrRestore(_contentResults);
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
                CreateAlertDialog("", errorMessage + e.Message);
            }
        }


        //подключаемся к Google диску
        public void OnConnected(Bundle connectionHint)
        {
            DriveClass.DriveApi.NewDriveContents(_googleApiClient).SetResultCallback(this);
        }

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

        //если удачно авторизовались 
        void IResultCallback.OnResult(Java.Lang.Object result)
        {
            _contentResults = (result).JavaCast<IDriveApiDriveContentsResult>();
            DoWorkBackupOrRestore(_contentResults);
        }

        private async void DoWorkBackupOrRestore(IDriveApiDriveContentsResult contentResults)
        {
            try
            {
                _dialogService.ShowLoadDialog();
                if (contentResults != null)
                {
                    if (!contentResults.Status.IsSuccess) // handle the error
                        return;
                    await Task.Run(async () =>
                    {
                        if (isCreateBackUp)
                            CreateBackUpFolderAndFile(contentResults);
                        else
                            await GetBackUpFile(contentResults);
                        _googleApiClient.Disconnect();
                    });
                }
                else
                    CreateAlertDialog("", errorMessage);
            }
            catch (Exception e)
            {
                CreateAlertDialog("", errorMessage);
                Log.Logger.Error(e);
            }
            finally
            {
                _dialogService.HideLoadDialog();
            }
        }

 

        //получаем файл backup
        private async Task GetBackUpFile(IDriveApiDriveContentsResult contentResults)
        {
            try
            {
                var contentFile = new StringBuilder();
                //получаем папку backup            
                DriveId folderBackUpId = FindItems(folderName).Result;
                if (folderBackUpId == null)
                {
                    CreateAlertDialog("", errorMessage);
                    //папка с backup не обнаружена
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
                        //чтение файла из Google drive и получение строки в BAse64       
                        var readFile = await driveFile.OpenAsync(_googleApiClient, DriveFile.ModeReadOnly, null);
                        using (var inpstr = readFile.DriveContents.InputStream)
                        using (var streamReade = new StreamReader(inpstr))
                        {
                            while (streamReade.Peek() >= 0)
                            {
                                contentFile.Append(await streamReade.ReadLineAsync());
                            }
                        }
                        //конвертируем строку из base64 и записываем в файл БД(переписываем)
                        byte[] bytes = Convert.FromBase64String(contentFile.ToString());
                        System.IO.File.WriteAllBytes(pathToDb, bytes);
                        //restore backup local
                        if(await _restoreFunc.Invoke(pathToDb) )
                            CreateAlertDialog("", successMessage);
                        else
                            throw new Exception("Error restore backup");
                    }
                }
            }
            catch (Exception er)
            {
                Log.Logger.Error(er.Message, er.StackTrace);
                CreateAlertDialog("", errorMessage);
            }
        }

        private void CreateAlertDialog(string title, string message)
        {

            _dialogService.ShowToast(message);
            //RunOnUiThread(() =>
            //{
            //    AlertDialog.Builder builder;
            //    builder = new AlertDialog.Builder(this);
            //    builder.SetTitle(title);
            //    builder.SetMessage(message);
            //    builder.SetCancelable(false);
            //    builder.SetPositiveButton("OK", delegate { });
            //    builder.Show();
            //});
        }


        //создаем backup
        private void CreateBackUpFolderAndFile(IDriveApiDriveContentsResult contentResults)
        {
            try
            {
                //поиск папки с файлом резервной копии
                DriveId folderBackUpId = FindItems(folderName).Result;
                //если папка не найдена создаем папку в Google диске
                if (folderBackUpId == null)
                {
                    CreateFolder(folderName);
                    //получаем ID 
                    folderBackUpId = FindItems(folderName).Result;
                }
                //записываем файл в папку
                WriteFile(folderBackUpId, filename, contentResults);
            }
            catch (Exception er)
            {
                Log.Logger.Error(er.Message, er.StackTrace);
            }
        }

        //поиск папки для backup
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
                Log.Logger.Error(er.Message, er.StackTrace);
                return folderId;
            }
        }

        //создаем папку в Google
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
                Log.Logger.Error(er.Message, er.StackTrace);
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
            catch (Exception er)
            {
                CreateAlertDialog("", errorMessage + er.Message);
            }
        }


        public void OnConnectionSuspended(int cause)
        {
            CreateAlertDialog("", errorMessage);
            Log.Logger.Error("On connection suspend");
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