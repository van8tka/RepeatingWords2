using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Widget;
using RepeatingWords.CustomControls;
using RepeatingWords.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

#pragma warning disable CS0612 // Type or member is obsolete
[assembly: ExportRenderer(typeof(EntryRepeatingWord), typeof(EntryRepeatingWordRender))]
#pragma warning restore CS0612 // Type or member is obsolete
namespace RepeatingWords.Droid
{

    /// <summary>
    /// класс переопределяет Entry для ввода слова при изучении слов(добавляет border)
    /// </summary>
    [Obsolete]
    public class EntryRepeatingWordRender:EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var view = (EntryRepeatingWord)Element;                  
                CreateBorder(view);
                // my_cursor is the xml file name which we defined above
                IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");
                JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.entry_cursor);
                view.Focus();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "BorderColor")
            {
                var view = (EntryRepeatingWord)sender;
                CreateBorder(view);
            }
        }

        private void CreateBorder(EntryRepeatingWord view)
        {
            if (view.IsCurvedCornersEnabled)
            {
                // creating gradient drawable for the curved background  
                var _gradientBackground = new GradientDrawable();
                _gradientBackground.SetShape(ShapeType.Rectangle);
                _gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

                // Thickness of the stroke line  
                _gradientBackground.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());

                // Radius for the curves  
                _gradientBackground.SetCornerRadius(
                    DpToPixels(this.Context, Convert.ToSingle(view.CornerRadius)));

                // set the background of the   
                Control.SetBackground(_gradientBackground);
            }
            // Set padding for the internal text from border  
            Control.SetPadding(
                (int)DpToPixels(this.Context, Convert.ToSingle(12)), Control.PaddingTop,
                (int)DpToPixels(this.Context, Convert.ToSingle(12)), Control.PaddingBottom);
        }

      
        public static float DpToPixels(Context context, float valueInDp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
        }
    }
}