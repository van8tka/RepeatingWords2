using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class BackupGoogleService : IBackupService
    {
        public BackupGoogleService(IExport export, IImport import)
        {
            _export = export;
            _import = import;
        }
        private readonly IImport _import;
        private readonly IExport _export;

        public Task<bool> CreateBackup(string file)
        {
            try
            {
                DependencyService.Get<IGoogleDriveWorker>().CreateBackupGoogleDrive(_export, file, Constants.LOCAL_FOLDER_BACKUP);
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return Task.FromResult(false);
            }
        }

        public Task<bool> RestoreBackup(string file)
        {
            try
            {
                DependencyService.Get<IGoogleDriveWorker>().RestoreBackupGoogleDriveFile(_import,  file, Constants.LOCAL_FOLDER_BACKUP);
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
