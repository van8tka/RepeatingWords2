using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Gms.Common.Apis;
using Android.Gms.Drive;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Xamarin.Forms;
using RepeatingWords.LoggerService;

namespace RepeatingWords.Droid
{

    [Activity(Label = "Cards of words", MainLauncher = true, Theme = "@style/MyTheme.Splash", Icon = "@mipmap/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //для установки SplashScreen обязательно использовать FormsAppCompatActivity а не FormsApplicationActivity
    public partial class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, GoogleApiClient.IConnectionCallbacks, IResultCallback, IDriveApiDriveContentsResult
    {

        internal static MainActivity Instance { get; private set; }
     

        public static bool HasPermissionToReadWriteExternalStorage = false;
      //  private const string APPLICATION_CODE = "ca-app-pub-5993977371632312~7094124560";

        protected override void OnCreate(Bundle bundle)
        {
            Android.Support.V7.App.AppCompatDelegate.CompatVectorFromResourcesEnabled = true;
            base.OnCreate(bundle);
            Instance = this;
            global::Xamarin.Forms.Forms.Init(this, bundle);
         //   Android.Gms.Ads.MobileAds.Initialize(this, APPLICATION_CODE);
            Task.Run(() =>
            {
                //проверка наличия разрешения
                CheckPermissionForStorage();
                UserDialogs.Init(this);
            });
            FormsControls.Droid.Main.Init(this);
            LoadApplication(new App( new SQLite_Android()));                   
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
            if (falseflag)
            {
                var ignore = new FitWindowsLinearLayout(Android.App.Application.Context);
            }
        }
    }
}

