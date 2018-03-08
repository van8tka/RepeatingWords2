using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;

namespace RepeatingWords.Droid
{

    [Activity(Label = "Cards of words", MainLauncher = true, Theme = "@style/MyTheme.Splash", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //для установки SplashScreen обязательно использовать FormsAppCompatActivity а не FormsApplicationActivity
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity         
    {
        public static bool HasPermissionToReadWriteExternalStorage = false;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
             //проверка наличия разрешения
                CheckPermissionForStorage();          
            LoadApplication(new App());
           
        }
        //переопределение кнопки назад
        public async override void OnBackPressed()
        {
            base.OnBackPressed();
        }
        //код получения разрешения
        const int REQUEST_STORAGE = 1010;
        //проверка наличия разрешения на доступ к памяти смартфона
        private void CheckPermissionForStorage()
        {
            try
            {
                if(ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage)==(int)Permission.Granted && ContextCompat.CheckSelfPermission(this,Manifest.Permission.WriteExternalStorage)==(int)Permission.Granted)
                {
                    HasPermissionToReadWriteExternalStorage = true;
                }
                else
                {
                    var requestPermission = new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage };
                    ActivityCompat.RequestPermissions(this, requestPermission, REQUEST_STORAGE);
                }
            }
            catch(Exception er)
            { }
        }
        //обработка результата установки разрешения
        public override void OnRequestPermissionsResult(int requestCode,string[] permissions, Permission[] grantResults)
        {
            try
            {
                if(requestCode==REQUEST_STORAGE)
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
            catch(Exception er)
            { }
        }
        


    }
}

