using RepeatingWords.Helpers.Interfaces;
using System;
using RepeatingWords.LoggerService;
using RepeatingWords.Heleprs;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
  
    public class ThemeChangeService:IThemeService
    {       
        public bool GetCurrentTheme()
        {
            try
            {
                object propThem;
                bool isDark = false;
                if (App.Current.Properties.TryGetValue(Constants.THEME, out propThem))
                {
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
                    App.Current.Properties.Add(Constants.THEME, Constants.THEME_WHITE);
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
                object propThem;
                string nameTheme = string.Empty;
                if (App.Current.Properties.TryGetValue(Constants.THEME, out propThem))
                {
                    if (propThem.Equals(Constants.THEME_WHITE))
                        nameTheme = SetDarkTheme();
                    else
                        nameTheme = SetWhiteTheme();
                }
                else
                    throw new ArgumentException("Theme wasn't set");
                App.Current.Properties.Remove(Constants.THEME);
                App.Current.Properties.Add(Constants.THEME, nameTheme);
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
            Application.Current.Resources["LableHeadApp"] = Application.Current.Resources["LableHeadAppBlack"];
            Application.Current.Resources["LabelColor"] = Application.Current.Resources["LabelNavy"];
            Application.Current.Resources["PickerColor"] = Application.Current.Resources["PickerColorNavy"];
            Application.Current.Resources["LabelColorWB"] = Application.Current.Resources["LabelBlack"];
            Application.Current.Resources["ColorWB"] = Application.Current.Resources["ColorBlack"];
            Application.Current.Resources["ColorBlGr"] = Application.Current.Resources["ColorBlue"];
            return Constants.THEME_WHITE;
        }
        private string SetDarkTheme()
        {
            Application.Current.Resources["TitleApp"] = Application.Current.Resources["TitleAppBlack"];
            Application.Current.Resources["LableHeadApp"] = Application.Current.Resources["LableHeadAppWhite"];
            Application.Current.Resources["LabelColor"] = Application.Current.Resources["LabelYellow"];
            Application.Current.Resources["LabelColorWB"] = Application.Current.Resources["LabelWhite"];
            Application.Current.Resources["PickerColor"] = Application.Current.Resources["PickerColorYellow"];
            Application.Current.Resources["ColorWB"] = Application.Current.Resources["ColorWhite"];
            Application.Current.Resources["ColorBlGr"] = Application.Current.Resources["ColorYellow"];
            return Constants.THEME_DARK;
        }

     
    }
}
