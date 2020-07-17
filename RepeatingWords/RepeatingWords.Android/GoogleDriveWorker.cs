using System;
using System.Threading.Tasks;
using Android.Content;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.Services;
using Xamarin.Forms;


[assembly: Dependency(typeof(RepeatingWords.Droid.GoogleDriveWorker))]
namespace RepeatingWords.Droid
{
    public class GoogleDriveWorker : IGoogleDriveWorker             
    {
      
        public bool CreateBackupGoogleDrive(string folderName, string fileName, string pathToDb, string succesMessage, string errorMessage, IDialogService dialogService)
        {
           // MainActivity.Instance.GoogleCustomAuthorithation(true, dialogService ,folderName, fileName, pathToDb, succesMessage, errorMessage);
           // MainActivity.Instance.GoogleCustomAuth();
            return true;
        }

        public bool RestoreBackupGoogleDriveFile(IImport import, string partOfFileNameBackup, string folderName)
        {
            MainActivity.Instance.GoogleCustomAuth(import,folderName, partOfFileNameBackup);
            // MainActivity.Instance.GoogleCustomAuthorithation(false, dialogService, folderName:folderName, fileName:partOfFileNameBackup, pathToDb: filePathToDbFull, successMessage: successMessage, errorMessage: errorMessage, restoreLocaleFunc);
            return true;
        }
    }
}