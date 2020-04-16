using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepeatingWords.Model;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace RepeatingWords.Services
{
    public class VolumeLanguageService : IVolumeLanguageService
    {
        public VolumeLanguageService()
        {
            GetLocales();
        }

        private async void GetLocales()
        {
            Locales = await TextToSpeech.GetLocalesAsync();
        }

        public static IEnumerable<Locale> Locales;
         

        public VolumeLanguageModel GetVolumeLanguage()
        {
            object volumeLanguage;
            if (App.Current.Properties.TryGetValue(Constants.VOLUME_LANGUAGE, out volumeLanguage))
            {
                if(volumeLanguage is VolumeLanguageModel languageSpeaker)
                    return languageSpeaker;
            }
            var currentLocale = Locales?.Where(x => x.Country == "GB").FirstOrDefault();
            if (currentLocale == null)
                throw new ArgumentNullException(nameof(currentLocale));
            var lang = new VolumeLanguageModel() { Name = currentLocale.Name, CountryCode = currentLocale.Country, LanguageCode = currentLocale.Language, IsChecked = Color.FromHex("#6bafef") };
            App.Current.Properties.Add(Constants.VOLUME_LANGUAGE, lang);
            return lang;
        }

        public bool SetVolumeLanguage(VolumeLanguageModel languageSpeaker)
        {
            try
            {
                if (App.Current.Properties.ContainsKey(Constants.VOLUME_LANGUAGE))
                {
                    App.Current.Properties.Remove(Constants.VOLUME_LANGUAGE);
                }
                App.Current.Properties.Add(Constants.VOLUME_LANGUAGE, languageSpeaker);
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
