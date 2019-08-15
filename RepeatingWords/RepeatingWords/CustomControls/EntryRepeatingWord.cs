using Xamarin.Forms;

namespace RepeatingWords.CustomControls
{
    public class EntryRepeatingWord:Entry
    {
      

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(EntryRepeatingWord), Color.Gray);
        public Color BorderColor { get=>(Color)GetValue(BorderColorProperty); set=>SetValue(BorderColorProperty, value); }
        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(EntryRepeatingWord), Device.OnPlatform<int>(1, 2, 2));
        public int BorderWidth { get => (int)GetValue(BorderWidthProperty);  set => SetValue(BorderWidthProperty, value);  }
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(EntryRepeatingWord), Device.OnPlatform<double>(6, 7, 7));    
        public double CornerRadius { get => (double)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }
        public static readonly BindableProperty IsCurvedCornersEnabledProperty = BindableProperty.Create(nameof(IsCurvedCornersEnabled), typeof(bool), typeof(EntryRepeatingWord), true);     
        public bool IsCurvedCornersEnabled { get => (bool)GetValue(IsCurvedCornersEnabledProperty); set => SetValue(IsCurvedCornersEnabledProperty, value); }
    }
}
