using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IGoogleDriveWorker
    {
        bool CreateBackupGoogleDrive(string folderName, string fileName, string filePathToDbFull, string successMsg, string errorMsg, IDialogService dialogService);
        //  bool RestoreBackupGoogleDriveFile(string filePathToDbFull, string fileName,string folderName,string successMsg, string errorMsg, Func<string, Task<bool>> restoreLocaleFunc, IDialogService dialogService);
        bool RestoreBackupGoogleDriveFile(IImport import, string partOfFileNameBackup, string folderName);
    }
}