using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class BackupGoogleService : IBackupService
    {
        public BackupGoogleService(BackupLocalService localBackup)
        {
            _localBaclupService = localBackup;
        }

        private readonly BackupLocalService _localBaclupService;

        public async Task<bool> CreateBackup(string file)
        {
            try
            {
                //создаем локальный бэкап
                await _localBaclupService.CreateBackup(file);
                //копируем в GoogleDrive
                bool success = DependencyService.Get<IGoogleDriveWorker>().CreateBackupGoogleDrive(Constants.LOCAL_FOLDER_BACKUP, file, _localBaclupService.PathToLastBackupFile, Resource.BackupWasCreatedGoogle, Resource.BackUpErrorCreated);
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return false;
            }
        }

        public async Task<bool> RestoreBackup(string file)
        {
            try
            {
                //получаем путь к локальному бэкапу
                string filePathDefault = DependencyService.Get<IFileWorker>().CreateFolder(
                    Constants.LOCAL_FOLDER_BACKUP,
                    "fromGDrivebackup" + DateTime.Now.ToString("ddMMyyyy_hhmm") + ".json");
                //копируем бэкап из GooglDrive в локальную папку
                bool success = DependencyService.Get<IGoogleDriveWorker>().RestoreBackupGoogleDriveFile(filePathDefault, file, Constants.LOCAL_FOLDER_BACKUP, Resource.BackupRestored, Resource.BackUpErrorRestored);
                //делаем бэкап из локального файла
                await _localBaclupService.RestoreBackup(filePathDefault);
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return false;
            }
        }
    }
}
