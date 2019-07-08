using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class BackupLocalService : IBackupService
    {
        //ctor
        public BackupLocalService(ISQLite sqlite)
        {
            _sqlite = sqlite;
        }

        private readonly ISQLite _sqlite;

        public Task<bool> CreateBackup(string file)
        {
            try
            {//получим путь к папке
                Log.Logger.Info("begin create local backup");
                string titleSuccess = Resource.SuccessStr;
                string backUpWasCreated = Resource.BackupWasCreatedInFolder + " ";
                string filePathDefault = DependencyService.Get<IFileWorker>().CreateFolder(Constants.LOCAL_FOLDER_BACKUP , file);
                //создаем резервную копию передаем путь к БД и путь для сохранения резервной копиии
                bool succes = DependencyService.Get<IFileWorker>().WriteFile(_sqlite.GetDatabasePath(Constants.DATABASE_NAME), filePathDefault);               
                return Task.FromResult( succes );
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }

        public async Task<bool> RestoreBackup(string file)
        {
            try
            {
                string fileBackUp = await DependencyService.Get<IFileWorker>().GetBackUpFilesAsync(Constants.LOCAL_FOLDER_BACKUP);
                bool success = false;
                if (!string.IsNullOrEmpty(fileBackUp))
                    success = DependencyService.Get<IFileWorker>().WriteFile(fileBackUp, _sqlite.GetDatabasePath(Constants.DATABASE_NAME));
                return success;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }
    }
}
