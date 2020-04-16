using System;
using System.Collections.Generic;
using System.Text;
using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using Xamarin.Essentials;

namespace RepeatingWords.Services
{
   public class ShowLanguageService:IShowLanguage
    {
        public bool GetFirstLanguage()
        {
            try
            {
               
                bool isFromNative = false;
                if (Preferences.ContainsKey(Constants.LANGUAGE_FIRST))
                {
                    string propThem = Preferences.Get(Constants.LANGUAGE_FIRST, "");
                    if (propThem.Equals(Constants.LANGUAGE_FOREIGN))
                        isFromNative = false;
                    else
                        isFromNative = true;
                }
                else
                {
                    isFromNative = false;
                    Preferences.Set(Constants.LANGUAGE_FIRST, Constants.LANGUAGE_FOREIGN);
                }
                return isFromNative;
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }

        public bool ChangeFirstLanguage()
        {
            if (GetFirstLanguage())
            {
                //set foreign
                Preferences.Remove(Constants.LANGUAGE_FIRST);
                Preferences.Set(Constants.LANGUAGE_FIRST, Constants.LANGUAGE_FOREIGN);
                return false;
            }
            else
            {
                //set native
                Preferences.Remove(Constants.LANGUAGE_FIRST);
                Preferences.Set(Constants.LANGUAGE_FIRST, Constants.LANGUAGE_NATIVE);
                return true;
            }
        }
    }
}
