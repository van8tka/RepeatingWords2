using Xamarin.Forms;

namespace RepeatingWords.CustomControls
{
   public class ButtonAnimationScale : Button
    {
        public ButtonAnimationScale() : base()
        {
            this.Clicked += async (sender, e) =>
            {
                var btn = (ButtonAnimationScale)sender;
                await btn.ScaleTo(1.2, 250, Easing.SpringIn);
                await btn.ScaleTo(1, 250);
            };
        }
    }
}
