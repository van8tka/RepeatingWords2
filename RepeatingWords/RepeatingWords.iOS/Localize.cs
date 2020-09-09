using System;
using System.Globalization;
using Xamarin.Forms;
using Foundation;
 
[assembly: Dependency(typeof(RepeatingWords.iOS.Localize))]
namespace RepeatingWords.iOS
{
    public class Localize:ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var pref = NSLocale.PreferredLanguages[0];
                var netLanguage = pref.ToString().Replace('-','-');
                return new CultureInfo(netLanguage);
            }
            throw new ArgumentNullException("Error get loaclization, PreferredLanguages < 0");
        }
    }
}
