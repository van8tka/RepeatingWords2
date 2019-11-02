using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using RepeatingWords.Model;
 

namespace RepeatingWords.Services
{
    public class VolumeLanguageService : IVolumeLanguageService
    {
     
        public VolumeLanguageModel GetVolumeLanguage()
        {
            object volumeLanguage;
            if (App.Current.Properties.TryGetValue(Constants.VOLUME_LANGUAGE, out volumeLanguage))
            {
                if(volumeLanguage is VolumeLanguageModel languageSpeaker)
                    return languageSpeaker;
            }
            return Constants.VOLUME_LANGUAGE_DEFAULT;
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
