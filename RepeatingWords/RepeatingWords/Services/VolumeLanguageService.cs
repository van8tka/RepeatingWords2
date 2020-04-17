﻿using RepeatingWords.Heleprs;
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
         

        public string GetVolumeLanguage()
        {
            if (Preferences.ContainsKey(Constants.VOLUME_LANGUAGE))
            {
                return Preferences.Get(Constants.VOLUME_LANGUAGE,"");
            }
            var currentLocale = Locales?.Where(x => x.Country == "GB").FirstOrDefault();
            if (currentLocale == null)
                throw new ArgumentNullException(nameof(currentLocale));
            Preferences.Set(Constants.VOLUME_LANGUAGE, currentLocale.Name);
            return currentLocale.Name;
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
