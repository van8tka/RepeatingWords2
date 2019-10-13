using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class BackupGoogleService : IBackupService
    {

        //ctor
        public BackupGoogleService(ISQLite sqlite)
        {
            _sqlite = sqlite;
        }

        private readonly ISQLite _sqlite;
      
        public Task<bool> CreateBackup(string file)
        {
            try
            {
                //передаем название папки бэкапа.название файла которые нужно создать и путь к файлу БД
                bool success = DependencyService.Get<IGoogleDriveWorker>().CreateBackupGoogleDrive(Constants.LOCAL_FOLDER_BACKUP, file, _sqlite.GetDatabasePath(Constants.DATABASE_NAME), Resource.BackupWasCreatedGoogle, Resource.BackUpErrorCreated);
                return Task.FromResult(success);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }

        public Task<bool> RestoreBackup(string file)
        {
            try
            {
               bool success = DependencyService.Get<IGoogleDriveWorker>().RestoreBackupGoogleDriveFile(_sqlite.GetDatabasePath(Constants.DATABASE_NAME), file, Constants.LOCAL_FOLDER_BACKUP, Resource.BackupRestored, Resource.BackUpErrorRestored);
                return Task.FromResult(success);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }
    }
}
