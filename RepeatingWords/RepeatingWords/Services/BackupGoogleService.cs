using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Threading.Tasks;
using RepeatingWords.Interfaces;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class BackupGoogleService : IBackupService
    {
        public BackupGoogleService(BackupLocalService localBackup, IDialogService dialogService)
        {
            _localBaclupService = localBackup;
            _dialogService = dialogService;
        }

        private readonly IDialogService _dialogService;
        private readonly BackupLocalService _localBaclupService;

        public async Task<bool> CreateBackup(string file)
        {
            try
            {
                //создаем локальный бэкап
                await _localBaclupService.CreateBackup(file);
                //копируем в GoogleDrive
                bool success = DependencyService.Get<IGoogleDriveWorker>().CreateBackupGoogleDrive(Constants.LOCAL_FOLDER_BACKUP, file, _localBaclupService.PathToLastBackupFile, Resource.BackupWasCreatedGoogle, Resource.BackUpErrorCreated, _dialogService);
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return false;
            }
        }

        public Task<bool> RestoreBackup(string file)
        {
            try
            {
                //получаем путь к локальному бэкапу
                string filePathDefault = DependencyService.Get<IFileWorker>().CreateFolder(Constants.LOCAL_FOLDER_BACKUP,"fromGDrivebackup" + DateTime.Now.ToString("ddMMyyyy_hhmm") + ".json");
                //копируем бэкап из GooglDrive в локальную папку
                Func<string, Task<bool>> restoreFunc = (str) => _localBaclupService.RestoreBackup(str);
                bool success = DependencyService.Get<IGoogleDriveWorker>().RestoreBackupGoogleDriveFile(filePathDefault, file, Constants.LOCAL_FOLDER_BACKUP, Resource.BackupRestored, Resource.BackUpErrorRestored, restoreFunc, _dialogService);
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return Task.FromResult(false);
            }
        }
    }
}
