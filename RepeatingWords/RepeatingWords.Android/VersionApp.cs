using System;
using RepeatingWords.Helpers.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(RepeatingWords.Droid.VersionApp))]
namespace RepeatingWords.Droid
{
    public class VersionApp : IVersionApp
    {
        public float GetVersionApp()
        {
            float version = -1;
            var rawVersion = MainActivity.Instance.BaseContext.ApplicationContext.PackageManager
                .GetPackageInfo(MainActivity.Instance.BaseContext.PackageName, 0).VersionName;
            rawVersion = rawVersion.Replace('.', ',');
            Single.TryParse(rawVersion, out version);
            return version;
        }
    }
}