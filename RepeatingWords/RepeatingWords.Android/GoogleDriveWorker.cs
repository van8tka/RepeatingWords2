using RepeatingWords.Helpers.Interfaces;
using Xamarin.Forms;


[assembly: Dependency(typeof(RepeatingWords.Droid.GoogleDriveWorker))]
namespace RepeatingWords.Droid
{
    public class GoogleDriveWorker : IGoogleDriveWorker             
    {

        public void CreateBackupGoogleDrive(IExport exportLocal, string fileNameStart, string folderName)
        {
            MainActivity.Instance.GoogleCustomAuthExport(exportLocal,folderName, fileNameStart);
        }

        public void RestoreBackupGoogleDriveFile(IImport importLocal, string fileNameStart, string folderName)
        {
            MainActivity.Instance.GoogleCustomAuthImport(importLocal, folderName, fileNameStart);
        }
    }
}