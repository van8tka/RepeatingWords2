using RepeatingWords.Helpers.Interfaces;
using System;
using RepeatingWords.LoggerService;
using RepeatingWords.Heleprs;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
  
    public class ThemeChangeService:IThemeService
    {       
        public bool GetCurrentTheme()
        {
            try
            {
                bool isDark = false;
                if (Preferences.ContainsKey(Constants.THEME))
                {
                    string propThem = Preferences.Get(Constants.THEME, "");
                    if (propThem.Equals(Constants.THEME_WHITE))
                    {
                        SetWhiteTheme();                       
                    }
                    else
                    {
                        SetDarkTheme();
                        isDark = true;
                    }                        
                }
                else
                {                 
                    SetWhiteTheme();
                    Preferences.Set(Constants.THEME, Constants.THEME_WHITE);
                }
                return isDark;
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }


        public bool ChangeTheme()
        {
            try
            {
                string nameTheme = string.Empty;
                if ( Preferences.ContainsKey(Constants.THEME) )
                {
                    string propThem = Preferences.Get(Constants.THEME, "");
                    if (propThem.Equals(Constants.THEME_WHITE))
                        nameTheme = SetDarkTheme();
                    else
                        nameTheme = SetWhiteTheme();
                }
                else
                    throw new ArgumentException("Theme wasn't set");
                Preferences.Remove(Constants.THEME);
                Preferences.Set(Constants.THEME, nameTheme);
                return nameTheme.Equals(Constants.THEME_DARK) ? true : false;
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
                throw;
            }
        }


        private string SetWhiteTheme()
        {
            Application.Current.Resources["TitleApp"] = Application.Current.Resources["TitleAppWhite"];
            Application.Current.Resources["LabelColor"] = Application.Current.Resources["LabelBlack"];
            Application.Current.Resources["LabelColorWB"] = Application.Current.Resources["LabelBlack"];
            Application.Current.Resources["ColorWB"] = Application.Current.Resources["ColorBlack"];
            Application.Current.Resources["BottomBarStyle"] = Application.Current.Resources["BottomBarStyleLight"];
            return Constants.THEME_WHITE;
        }
        private string SetDarkTheme()
        {
            Application.Current.Resources["TitleApp"] = Application.Current.Resources["TitleAppBlack"];
            Application.Current.Resources["LabelColor"] = Application.Current.Resources["LabelWhite"];
            Application.Current.Resources["LabelColorWB"] = Application.Current.Resources["LabelWhite"];
            Application.Current.Resources["ColorWB"] = Application.Current.Resources["ColorWhite"];
            Application.Current.Resources["BottomBarStyle"] = Application.Current.Resources["BottomBarStyleDark"];
            return Constants.THEME_DARK;
        }

     
    }
}
