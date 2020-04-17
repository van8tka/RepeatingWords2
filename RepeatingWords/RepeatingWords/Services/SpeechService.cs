using System;
using System.Diagnostics;
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

        private Task SetSpeechLocale()
        {
            return Task.Run(() =>
            {
                var locales = VolumeLanguageService.Locales;
                _language = _volumeService.GetVolumeLanguage();
                _settings = new SpeechOptions()
                {
                    Volume = .75f,
                    Pitch = 1.0f,
                    Locale = locales.FirstOrDefault(x => x.Name.Equals(_language, StringComparison.OrdinalIgnoreCase))
                };
            });
        }

        private SpeechOptions _settings;
        private string _language;
        public string Language => _language.ToLower();

        public async Task Speak(string text)
        {
            Log.Logger.Info($"SpeechService Speak - {text} +language- {_language}");
            Debug.Assert(_settings!=null);
            await TextToSpeech.SpeakAsync(text, _settings);
        }
    }
}
