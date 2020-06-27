using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Gms.Common.Apis;
using Android.Gms.Drive;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using AndroidX.AppCompat.Widget;
using RepeatingWords.Droid.LoggerService;
using RepeatingWords.LoggerService;
 

namespace RepeatingWords.Droid
{
 

[Activity(Label = "Cards of words", MainLauncher = true, Theme = "@style/MyTheme.Splash", Icon = "@mipmap/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //для установки SplashScreen обязательно использовать FormsAppCompatActivity а не FormsApplicationActivity
    public partial class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, GoogleApiClient.IConnectionCallbacks, IResultCallback, IDriveApiDriveContentsResult
    {
        internal static MainActivity Instance { get; private set; }
        public static bool HasPermissionToReadWriteExternalStorage = false;

        protected override void OnCreate(Bundle bundle)
        {
            LinkerPleaseInclude();
            AndroidX.AppCompat.App.AppCompatDelegate.CompatVectorFromResourcesEnabled = true;
            base.OnCreate(bundle);
            Instance = this;
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Task.Run(() =>
            {
                //проверка наличия разрешения
                CheckPermissionForStorage();
                UserDialogs.Init(this);
            });
            //!!!REMOVE Nuget Packages AnimationNavigationPage
            //Build and Reference library on  E:\Visual\AnimationNavigationPage called FormsControls.Base and .Droid
            FormsControls.Droid.Main.Init(this);
            var logManager = new NLogManager().GetLog();
            var log = new Log(logManager);
            logManager.Info("____NEW SESSION CARDS OF WORDS___");
            LoadApplication(new App(new SQLite_Android(), log));
        }
        
        //код получения разрешения
        const int REQUEST_STORAGE = 1010;
        //проверка наличия разрешения на доступ к памяти смартфона
        private void CheckPermissionForStorage()
        {
            try
            {
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted && ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted)
                {
                    HasPermissionToReadWriteExternalStorage = true;
                }
                else
                {
                    var requestPermission = new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage };
                    ActivityCompat.RequestPermissions(this, requestPermission, REQUEST_STORAGE);
                }
            }
            catch (Exception er)
            {
                Log.Logger.Error("CheckPermissionForStorage", er.Message);
            }
        }
        //обработка результата установки разрешения
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                if (requestCode == REQUEST_STORAGE)
                {
                    if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
                        HasPermissionToReadWriteExternalStorage = true;
                    else
                        HasPermissionToReadWriteExternalStorage = false;
                }
                else
                {
                    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                    HasPermissionToReadWriteExternalStorage = false;
                }
            }
            catch (Exception er)
            {
                Log.Logger.Error("OnRequestPermissionsResult", er.Message);
            }
        }


        //this fake method for include 
        static bool falseflag = false;
        static void LinkerPleaseInclude()
        {
            MobileAdsInitProvider intiPr;
            string tt;
            if (falseflag)
            {
                var ignore = new FitWindowsLinearLayout(Android.App.Application.Context);
                intiPr = new MobileAdsInitProvider();
                tt = intiPr.CallingPackage;
            }

           
        }
    }
}

