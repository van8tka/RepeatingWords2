using System;
using System.Collections.Generic;
using System.Text;
using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;

namespace RepeatingWords.Services
{
   public class FirstLanguageService:IFirstLanguage
    {
        public bool GetFirstLanguage()
        {
            try
            {
                object propThem;
                bool isFromNative = false;
                if (App.Current.Properties.TryGetValue(Constants.LANGUAGE_FIRST, out propThem))
                {
                    if (propThem.ToString().Equals(Constants.LANGUAGE_FOREIGN))
                        isFromNative = false;
                    else
                        isFromNative = true;
                }
                else
                {
                    isFromNative = false;
                    App.Current.Properties.Add(Constants.LANGUAGE_FIRST, Constants.LANGUAGE_FOREIGN);
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
                App.Current.Properties.Remove(Constants.LANGUAGE_FIRST);
                App.Current.Properties.Add(Constants.LANGUAGE_FIRST, Constants.LANGUAGE_FOREIGN);
                return false;
            }
            else
            {
                //set native
                App.Current.Properties.Remove(Constants.LANGUAGE_FIRST);
                App.Current.Properties.Add(Constants.LANGUAGE_FIRST, Constants.LANGUAGE_NATIVE);
                return true;
            }
        }
    }
}
