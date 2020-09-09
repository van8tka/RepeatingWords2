using Xamarin.Forms;
using RepeatingWords;
using RepeatingWords.iOS;
using Xamarin.Forms.Platform.iOS;
using Foundation;
[assembly: ExportRenderer(typeof(KeyButton), typeof(KeyButtonRender))]
namespace RepeatingWords.iOS
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class KeyButtonRender : ButtonRenderer
    {
        //??????????????? ???????? Button (??? ????? ? ????? ????????)
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

    }
#pragma warning restore CS0618 // Type or member is obsolete
}
