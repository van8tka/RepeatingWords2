using Xamarin.Forms;


[assembly: Dependency(typeof(RepeatingWords.Droid.GoogleDriveWorker))]
namespace RepeatingWords.Droid
{
    public class GoogleDriveWorker : IGoogleDriveWorker             
    {
      
        public bool CreateBackupGoogleDrive(string folderName, string fileName, string pathToDb)
        {
            bool isCreateBackUp = true;
            var activity = (MainActivity)Forms.Context;
            activity.GoogleCustomAuthorithation(isCreateBackUp, folderName, fileName, pathToDb);
            return true;
        }

        public bool RestoreBackupGoogleDriveFile(string filePathToDbFull,string partOfFileNameBackup,string folderName)
        {
                bool isCreateBackUp = false;
                var activity = (MainActivity)Forms.Context;
                activity.GoogleCustomAuthorithation(isCreateBackUp,folderName:folderName, fileName:partOfFileNameBackup, pathToDb: filePathToDbFull);
                return true;
           
        }
    }
}