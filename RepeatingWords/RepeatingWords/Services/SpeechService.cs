using System;
using System.Linq;
using System.Threading.Tasks;
using Plugin.TextToSpeech;
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
        }

 


        public  async Task Speak(string text)
        {
            try
            {
                var locales = await CrossTextToSpeech.Current.GetInstalledLanguages();
                var current = _volumeService.GetSysAbbreviationVolumeLanguage().Split('_');
                var locale = locales.FirstOrDefault(x => x.Language.Equals(current[0], StringComparison.OrdinalIgnoreCase));
                if (Device.RuntimePlatform == Device.Android)
                    await CrossTextToSpeech.Current.Speak(text, locale);
                //  if (Device.RuntimePlatform == Device.iOS)
                else
                    throw new Exception("Can't set the voice language");
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }
    }
}
