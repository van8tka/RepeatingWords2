using System;
using System.Linq;
using System.Threading.Tasks;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class SpeechService : ITextToSpeech
    {
        private readonly IVolumeLanguageService _volumeService;

        public SpeechService(IVolumeLanguageService volumeService)
        {
            _volumeService = volumeService ?? throw new ArgumentNullException(nameof(volumeService));
            SetSpeechLocale();
        }

        private async Task SetSpeechLocale()
        {
            var locales = VolumeLanguageService.Locales;
            var current = _volumeService.GetVolumeLanguage();
            _language = current.Name;
            _settings = new SpeechOptions()
            {
                Volume = .75f,
                Pitch = 1.0f,
                Locale = locales.FirstOrDefault(x =>
                    x.Language.Equals(current.LanguageCode, StringComparison.OrdinalIgnoreCase))
            };
        }

        private SpeechOptions _settings;
        private string _language;
        public string Language => _language.ToLower();

        public async Task Speak(string text)
        {
            Log.Logger.Info($"SpeechService Speak - {text} +language- {_settings.Locale.Language}");
            if (Device.RuntimePlatform == Device.Android)
                await TextToSpeech.SpeakAsync(text, _settings);
            //else if (Device.RuntimePlatform == Device.iOS)
            //    throw new Exception("Can't set the voice language");
            //else if (Device.RuntimePlatform == Device.UWP)
            //    throw new Exception("Can't set the voice language");
        }
    }
}
