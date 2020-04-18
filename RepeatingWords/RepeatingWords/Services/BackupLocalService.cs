using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class BackupLocalService : IBackupService
    {
        public BackupLocalService(IExport exportJson, IImport importJson)
        {
            _exportJson = exportJson;
            _importJson = importJson;
        }

        private readonly IExport _exportJson;
        private readonly IImport _importJson;

        public string PathToLastBackupFile { get; private set; }

        public async Task<bool> CreateBackup(string file)
        {
            try
            {
                var jsobject = await _exportJson.Export();
                string filePathDefault = DependencyService.Get<IFileWorker>().CreateFolder(Constants.LOCAL_FOLDER_BACKUP, file);
                File.WriteAllText(filePathDefault, jsobject.ToString());
                PathToLastBackupFile = filePathDefault;
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
                string fileBackUp;
                if (string.IsNullOrEmpty(file))
                    fileBackUp = await DependencyService.Get<IFileWorker>()
                        .GetBackUpFilesAsync(Constants.LOCAL_FOLDER_BACKUP);
                else
                    fileBackUp = file;
                if (!string.IsNullOrEmpty(fileBackUp))
                {
                    string jsonStr = File.ReadAllText(fileBackUp);
                    return await _importJson.import(JObject.Parse(jsonStr));
                }
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
