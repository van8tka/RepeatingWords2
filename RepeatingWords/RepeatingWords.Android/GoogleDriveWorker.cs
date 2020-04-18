using System;
using System.Threading.Tasks;
using Android.Content;
using RepeatingWords.Interfaces;
using Xamarin.Forms;


[assembly: Dependency(typeof(RepeatingWords.Droid.GoogleDriveWorker))]
namespace RepeatingWords.Droid
{
    public class GoogleDriveWorker : IGoogleDriveWorker             
    {
      
        public bool CreateBackupGoogleDrive(string folderName, string fileName, string pathToDb, string succesMessage, string errorMessage, IDialogService dialogService)
        {
            bool isCreateBackUp = true;
#pragma warning disable CS0618 // Type or member is obsolete
            var activity = (MainActivity)Forms.Context;
#pragma warning restore CS0618 // Type or member is obsolete
            activity.GoogleCustomAuthorithation(isCreateBackUp, dialogService ,folderName, fileName, pathToDb, succesMessage, errorMessage);
            return true;
        }

        public bool RestoreBackupGoogleDriveFile(string filePathToDbFull,string partOfFileNameBackup,string folderName, string successMessage, string errorMessage, Func<string, Task<bool>> restoreLocaleFunc, IDialogService dialogService)
        {
                bool isCreateBackUp = false;
#pragma warning disable CS0618 // Type or member is obsolete
            var activity = (MainActivity)Forms.Context;
#pragma warning restore CS0618 // Type or member is obsolete
            activity.GoogleCustomAuthorithation(isCreateBackUp, dialogService, folderName:folderName, fileName:partOfFileNameBackup, pathToDb: filePathToDbFull, successMessage: successMessage, errorMessage: errorMessage, restoreLocaleFunc);
                return true;
           
        }
    }
}