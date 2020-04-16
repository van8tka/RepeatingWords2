using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using Xamarin.Essentials;


namespace RepeatingWords.Services
{
    public class KeyboardTranscriptionChangeService : IKeyboardTranscriptionService
    {
        public bool ChangeUsingTranscriptionKeyboard()
        {
            try
            {
              
                string setKeyboard = string.Empty;
                bool isShow;
                if (Preferences.ContainsKey(Constants.KEYBOARD_TRANSCRIPTION))
                {
                    string propTrKeyb = Preferences.Get(Constants.KEYBOARD_TRANSCRIPTION, "");
                    if (propTrKeyb.Equals(Constants.KEYBOARD_TRANSCRIPTION_SHOWED))
                    {
                        setKeyboard = Constants.KEYBOARD_TRANSCRIPTION_HIDE;
                        isShow = false;
                    }
                    else
                    {
                        setKeyboard = Constants.KEYBOARD_TRANSCRIPTION_SHOWED;
                        isShow = true;
                    }
                }
                else
                    throw new ArgumentException("Transcription keyboard change wasn't set");
                Preferences.Remove(Constants.KEYBOARD_TRANSCRIPTION);
                Preferences.Set(Constants.KEYBOARD_TRANSCRIPTION, setKeyboard);
                return isShow;
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }          
        }

        public bool GetCurrentTranscriptionKeyboard()
        {
            try
            {
              
                bool isShow = false;
                if (Preferences.ContainsKey(Constants.KEYBOARD_TRANSCRIPTION))
                {
                    string propTrKeyb = Preferences.Get(Constants.KEYBOARD_TRANSCRIPTION, "");
                    if (propTrKeyb.Equals(Constants.KEYBOARD_TRANSCRIPTION_SHOWED))
                        isShow = true;
                }
                else
                {
                     Preferences.Set(Constants.KEYBOARD_TRANSCRIPTION, Constants.KEYBOARD_TRANSCRIPTION_SHOWED);
                    isShow = true;
                }
                return isShow;
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }
    }
}
