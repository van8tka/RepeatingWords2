﻿using Xamarin.Forms;


[assembly: Dependency(typeof(RepeatingWords.Droid.GoogleDriveWorker))]
namespace RepeatingWords.Droid
{
    public class GoogleDriveWorker : IGoogleDriveWorker             
    {
      
        public bool CreateBackupGoogleDrive(string folderName, string fileName, string pathToDb, string succesMessage, string errorMessage)
        {
            bool isCreateBackUp = true;
#pragma warning disable CS0618 // Type or member is obsolete
            var activity = (MainActivity)Forms.Context;
#pragma warning restore CS0618 // Type or member is obsolete
            activity.GoogleCustomAuthorithation(isCreateBackUp, folderName, fileName, pathToDb, succesMessage, errorMessage);
            return true;
        }

        public bool RestoreBackupGoogleDriveFile(string filePathToDbFull,string partOfFileNameBackup,string folderName, string successMessage, string errorMessage)
        {
                bool isCreateBackUp = false;
#pragma warning disable CS0618 // Type or member is obsolete
            var activity = (MainActivity)Forms.Context;
#pragma warning restore CS0618 // Type or member is obsolete
            activity.GoogleCustomAuthorithation(isCreateBackUp,folderName:folderName, fileName:partOfFileNameBackup, pathToDb: filePathToDbFull, successMessage: successMessage, errorMessage: errorMessage);
                return true;
           
        }
    }
}