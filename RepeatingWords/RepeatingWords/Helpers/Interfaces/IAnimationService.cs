using System.Threading.Tasks;
using Xamarin.Forms;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IAnimationService
    {
        Task<bool> AnimationPositionWord(SwipeDirection direction, Xamarin.Forms.View view);
        Task<bool> AnimationPositionWordRevert(Xamarin.Forms.View view);
        Task<bool> AnimationFade(Xamarin.Forms.View view, int fadeValue);
    }
}
