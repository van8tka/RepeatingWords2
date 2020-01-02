using System;
using System.Threading.Tasks;
using RepeatingWords.Helpers.Interfaces;
using Xamarin.Forms;

namespace RepeatingWords.Helpers
{
   public class LikeApplication
    {
        public static async Task Like(IDialogService _dialogService)
        {
            bool action = await _dialogService.ShowConfirmAsync(Resource.MessagePleaseReview, "", Resource.ButtonSendReview, Resource.ModalActCancel);
            if (action)
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                    {
                        Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=cardsofwords.cardsofwords"));
                        break;
                    }
                    case Device.UWP:
                    {
                        Device.OpenUri(new Uri("https://www.microsoft.com/store/apps/9n55bwkgshnf"));
                        break;
                    }
                    case Device.iOS:
                    {
                        // Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=cardsofwords.cardsofwords"));
                        break;
                    }
                }
            }
        }
    }
}
