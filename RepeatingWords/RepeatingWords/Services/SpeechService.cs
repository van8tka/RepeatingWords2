﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Plugin.TextToSpeech;
using Plugin.TextToSpeech.Abstractions;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
   public class SpeechService:ITextToSpeech
    {
        private readonly IVolumeLanguageService _volumeService;
        public SpeechService(IVolumeLanguageService volumeService)
        {
            _volumeService = volumeService ?? throw new ArgumentNullException(nameof(volumeService));
            SetSpeechLocale();
        }

        private async Task SetSpeechLocale()
        {
            var locales = await CrossTextToSpeech.Current.GetInstalledLanguages();
            var current = _volumeService.GetVolumeLanguage();
            _language = current.Name;
            _locale = locales.FirstOrDefault(x => x.Language.Equals(current.LanguageCode, StringComparison.OrdinalIgnoreCase));
        }

        private CrossLocale _locale;
        private string _language;
        public string Language => _language.ToLower();

        public async Task Speak(string text)
        {
            try
            {
                Log.Logger.Info($"SpeechService Speak - {text} +language- {_locale.Language}");
                if (Device.RuntimePlatform == Device.Android)
                    await CrossTextToSpeech.Current.Speak(text, _locale); 
                else if (Device.RuntimePlatform == Device.iOS)
                    throw new Exception("Can't set the voice language");
                else if (Device.RuntimePlatform == Device.UWP)
                    throw new Exception("Can't set the voice language");
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }
    }
}
