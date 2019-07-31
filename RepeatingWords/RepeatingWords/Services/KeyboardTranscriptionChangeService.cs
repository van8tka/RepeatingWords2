using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;


namespace RepeatingWords.Services
{
    public class KeyboardTranscriptionChangeService : IKeyboardTranscriptionService
    {
        public bool ChangeUsingTranscriptionKeyboard()
        {
            try
            {
                object propTrKeyb;
                string setKeyboard = string.Empty;
                bool isShow;
                if (App.Current.Properties.TryGetValue(Constants.KEYBOARD_TRANSCRIPTION, out propTrKeyb))
                {
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
                App.Current.Properties.Remove(Constants.KEYBOARD_TRANSCRIPTION);
                App.Current.Properties.Add(Constants.KEYBOARD_TRANSCRIPTION, setKeyboard);
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
                object propTrKeyb;
                bool isShow = false;
                if (App.Current.Properties.TryGetValue(Constants.KEYBOARD_TRANSCRIPTION, out propTrKeyb))
                {
                    if (propTrKeyb.Equals(Constants.KEYBOARD_TRANSCRIPTION_SHOWED))
                        isShow = true;
                }
                else
                {
                    App.Current.Properties.Add(Constants.KEYBOARD_TRANSCRIPTION, Constants.KEYBOARD_TRANSCRIPTION_SHOWED);
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
