using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;


namespace RepeatingWords.Services
{
    public class VolumeLanguageService : IVolumeLanguageService
    {
        public VolumeLanguageService()
        {
            GetLocales();
        }

        private static async void GetLocales()
        {
            var temp = await TextToSpeech.GetLocalesAsync();
            Locales = temp.ToList();
        }

        private static IList<Locale> _locale;
        public static IList<Locale> Locales
        {
            get
            {
                if (_locale == null)
                     GetLocales();
                return _locale;
            }
            private set => _locale = value;
        }
         

        public string GetVolumeLanguage()
        {
            try
            {
                if (Preferences.ContainsKey(Constants.VOLUME_LANGUAGE))
                {
                    return Preferences.Get(Constants.VOLUME_LANGUAGE, "");
                }
                var currentLocale = Locales?.FirstOrDefault();
                if (currentLocale == null)
                    throw new ArgumentNullException(nameof(currentLocale));
                Preferences.Set(Constants.VOLUME_LANGUAGE, currentLocale.Name);
                return currentLocale.Name;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }

        public bool SetVolumeLanguage(string languageName)
        {
            try
            {
                if (Preferences.ContainsKey(Constants.VOLUME_LANGUAGE))
                {
                    Preferences.Remove(Constants.VOLUME_LANGUAGE);
                }
                Preferences.Set(Constants.VOLUME_LANGUAGE, languageName);
                return true;
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }
    }
}
