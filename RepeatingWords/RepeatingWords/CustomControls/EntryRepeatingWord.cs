using System.ComponentModel;
using Xamarin.Forms;

namespace RepeatingWords.CustomControls
{
    public class EntryRepeatingWord:Entry
    {    
        public EntryRepeatingWord()
        {
            this.PropertyChanged += EntryPropertyChanged;
        }    
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(EntryRepeatingWord), Color.Gray,BindingMode.TwoWay, null, new BindableProperty.BindingPropertyChangedDelegate(HandleBorderColorChange));

        public Color BorderColor { get =>(Color)GetValue(BorderColorProperty); set { SetValue(BorderColorProperty, value);  }
        }
        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(EntryRepeatingWord), Device.OnPlatform<int>(1, 2, 2));
        public int BorderWidth { get => (int)GetValue(BorderWidthProperty);  set => SetValue(BorderWidthProperty, value);  }
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(EntryRepeatingWord), Device.OnPlatform<double>(6, 7, 7));    
        public double CornerRadius { get => (double)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }
        public static readonly BindableProperty IsCurvedCornersEnabledProperty = BindableProperty.Create(nameof(IsCurvedCornersEnabled), typeof(bool), typeof(EntryRepeatingWord), true);     
        public bool IsCurvedCornersEnabled { get => (bool)GetValue(IsCurvedCornersEnabledProperty); set => SetValue(IsCurvedCornersEnabledProperty, value); }

        private void EntryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BorderColor))
            {
                BorderColor = ((EntryRepeatingWord)sender).BorderColor;
            }
        }

          
        private static void HandleBorderColorChange(BindableObject bindable, object oldValue, object newValue)
        {
            if(newValue is Color)
            {
                ((EntryRepeatingWord)bindable).BorderColor = (Color)newValue;             
            }
               
        }
    }
}
