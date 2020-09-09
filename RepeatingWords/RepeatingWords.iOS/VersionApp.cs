using System;
using RepeatingWords.Helpers.Interfaces;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(RepeatingWords.iOS.VersionApp))]
namespace RepeatingWords.iOS
{
    public class VersionApp : IVersionApp
    {
        public float GetVersionApp()
        {
            float version = -1;
            var rawVersion = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();
            rawVersion = rawVersion.Replace('.', ',');
            Single.TryParse(rawVersion, out version);
            return version;
        }
    }
}