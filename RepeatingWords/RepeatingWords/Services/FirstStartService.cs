using System;
using System.Collections.Generic;
using System.Text;
using RepeatingWords.Heleprs;
using RepeatingWords.LoggerService;
using Xamarin.Essentials;

namespace RepeatingWords.Services
{
   internal class FirstStartService
    {
        public static bool IsFirstStart()
        {
            try
            {
                if (Preferences.ContainsKey(Constants.IS_FIRST_START))
                    return false;
                Preferences.Set(Constants.IS_FIRST_START, Constants.IS_FIRST_START);
                return true;
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }
    }
}
