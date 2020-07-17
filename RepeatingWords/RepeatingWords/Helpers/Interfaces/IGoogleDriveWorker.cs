namespace RepeatingWords.Helpers.Interfaces
{
    public interface IGoogleDriveWorker
    {
        void CreateBackupGoogleDrive(IExport export, string fileNameStart, string folderName);
        void RestoreBackupGoogleDriveFile(IImport import, string fileNameStart, string folderName);
    }
}