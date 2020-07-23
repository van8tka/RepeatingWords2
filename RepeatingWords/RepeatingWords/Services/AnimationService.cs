using System.Threading.Tasks;
using RepeatingWords.Helpers.Interfaces;
using Xamarin.Forms;

namespace RepeatingWords.Services
{
    public class AnimationService:IAnimationService
    {

        public Task AnimationPositionWord(SwipeDirection direction, Xamarin.Forms.View view)
        {
            switch (direction)
            {
                case SwipeDirection.Down:
                {
                    return Task.WhenAll(
                        view.TranslateTo(0, 100, 250, Easing.SinIn),
                        view.FadeTo(0, 250)
                    );
                }
                case SwipeDirection.Up:
                {
                    return Task.WhenAll(
                        view.TranslateTo(0, -100, 250, Easing.SinIn),
                        view.FadeTo(0, 250)
                    );
                }
                case SwipeDirection.Left:
                {
                   return  Task.WhenAll(
                        view.TranslateTo(-100, 0, 250, Easing.SinIn),
                        view.FadeTo(0, 250)
                    );
                }
                case SwipeDirection.Right:
                {
                  return Task.WhenAll(
                        view.FadeTo(0, 250),
                        view.TranslateTo(100, 0, 250, Easing.SinIn)
                    );
                }
                default: return Task.FromResult(false);
            }
        }


        public Task AnimationPositionWordRevert(Xamarin.Forms.View view)
        {
             return Task.WhenAll(
                view.TranslateTo(0, 0, 10),
                view.FadeTo(1, 150)
            );
        }


        public Task<bool> AnimationFade(Xamarin.Forms.View view, int opacity)
        {
             return AnimationFade(view, opacity, 150);
        }

        public Task<bool> AnimationFade(Xamarin.Forms.View view, int opacity, uint milliseconds)
        {
            return view.FadeTo(opacity, milliseconds);
        }
    }
}
