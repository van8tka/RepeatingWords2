using System.Threading.Tasks;
using RepeatingWords.Helpers.Interfaces;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class AnimationService:IAnimationService
    {

        public async Task<bool> AnimationPositionWord(SwipeDirection direction, Xamarin.Forms.View view)
        {
            switch (direction)
            {
                case SwipeDirection.Down:
                {
                    await Task.WhenAll(
                        view.TranslateTo(0, 100, 250, Easing.SinIn),
                        view.FadeTo(0, 250)
                    );
                    break;
                }
                case SwipeDirection.Up:
                {
                    await Task.WhenAll(
                        view.TranslateTo(0, -100, 250, Easing.SinIn),
                        view.FadeTo(0, 250)
                    );
                    break;
                }
                case SwipeDirection.Left:
                {
                    await Task.WhenAll(
                        view.TranslateTo(-100, 0, 250, Easing.SinIn),
                        view.FadeTo(0, 250)
                    );
                    break;
                }
                case SwipeDirection.Right:
                {
                    await Task.WhenAll(
                        view.FadeTo(0, 250),
                        view.TranslateTo(100, 0, 250, Easing.SinIn)
                    );
                    break;
                }
            }
            return await Task.FromResult(true);
        }


        public async Task<bool> AnimationPositionWordRevert(Xamarin.Forms.View view)
        {
            await Task.WhenAll(
                view.TranslateTo(0, 0, 10),
                view.FadeTo(1, 150)
            );
            return await Task.FromResult(true);
        }


        public async Task<bool> AnimationFade(Xamarin.Forms.View view, int opacity)
        {
             return await AnimationFade(view, opacity, 150);
        }

        public async Task<bool> AnimationFade(Xamarin.Forms.View view, int opacity, uint milliseconds)
        {
            return await view.FadeTo(opacity, milliseconds);
        }
    }
}
