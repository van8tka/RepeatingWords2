using System;
using System.Threading.Tasks;

namespace RepeatingWords
{
    public interface IGoogleDriveWorker
    {
        bool CreateBackupGoogleDrive(string folderName, string fileName, string filePathToDbFull, string successMsg, string errorMsg);
        bool RestoreBackupGoogleDriveFile(string filePathToDbFull, string fileName,string folderName,string successMsg, string errorMsg, Func<string, Task<bool>> restoreLocaleFunc);
    }
}
