using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RepeatingWords.Converters
{
    public class BoolenToColorConverter : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool isCheckedColor)
            {
                if(isCheckedColor)
                {
                    //  return Color.FromHex("#6bafef");
                    return Color.FromHex("#4badf9");
                }           
            }
             return Color.FromHex("#CDC9C9");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
           return this;
        }
    }
}
