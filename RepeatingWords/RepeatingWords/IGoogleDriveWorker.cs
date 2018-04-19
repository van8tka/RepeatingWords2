namespace RepeatingWords
{
    public interface IGoogleDriveWorker
    {
        bool CreateBackupGoogleDrive(string folderName, string fileName, string filePathToDbFull);
        bool RestoreBackupGoogleDriveFile(string filePathToDbFull, string fileName,string folderName);
    }
}
