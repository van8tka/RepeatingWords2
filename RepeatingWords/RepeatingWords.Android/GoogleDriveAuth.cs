using System;
using System.Threading.Tasks;
using Android.Widget;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Model;
using RepeatingWords.Services;

namespace RepeatingWords.Droid
{
    public partial class MainActivity: IGoogleAuthenticationDelegate
    {
        ///WORK WITH GOOGLE DRIVE
        //разные версии регистрации в googleApis в зависимости от sha1 
        //для debug - учетка в gooleApis с именем: debugAndroidV28
        //для release - clientcardsofwordsandroid
        public static AuthGoogle Auth;
        private string FOLDER_NAME { get; set; }
        private string FILE_START_NAME { get; set; }
        private IImport import { get; set; }
        private IExport export { get; set; }
        private bool STATE_RESTORE { get; set; }

        private void GoogleCustomAuth(string folderBackup, string fileStartNameBackup)
        {
            FILE_START_NAME = fileStartNameBackup;
            FOLDER_NAME = folderBackup;
            Auth = new AuthGoogle(Config.CLIENT_ID, Config.SCOPE, Config.REDIRECT_URL, this);
            var authenticator = Auth.GetAuthenticator();
            var intent = authenticator.GetUI(this);
            StartActivity(intent);
        }


        public void GoogleCustomAuthExport(IExport exportLocal, string fileNameStart, string folderName)
        {
            export = exportLocal;
            STATE_RESTORE = false;
            GoogleCustomAuth(fileNameStart, folderName);

        }

        public void GoogleCustomAuthImport(IImport importLocal, string fileNameStart, string folderName)
        {
            import = importLocal;
            STATE_RESTORE = true;
            GoogleCustomAuth(fileNameStart, folderName);
        }
 

        public async Task OnAuthenticationCompleted(GoogleOAuthToken token)
        {
            ShowToast("Success authorization on Google Drive");
            var gds = new GoogleDriveService();
            if (STATE_RESTORE)
            {
                if (await gds.GetBackupAsync(token,  FILE_START_NAME, FOLDER_NAME, import))
                    ShowToast("Success restore backup");
                else
                    ShowToast("Backup restore failed");
            }
            else
            {
                if (await gds.SetBackupAsync(token,  FILE_START_NAME, FOLDER_NAME, export))
                    ShowToast("Success create backup");
                else
                    ShowToast("Backup create failed");
            }
        }

        public void OnAuthenticationFailed(string message, Exception exception)
        {
           ShowToast("Error authorization on Google Drive");
        }

        public void OnAuthenticationCanceled()
        {
             ShowToast("Canceled authorization on Google Drive");
        }


        private void ShowToast(string msg)
        {
            Toast.MakeText(this, msg, ToastLength.Long).Show();
        }

    }
}
