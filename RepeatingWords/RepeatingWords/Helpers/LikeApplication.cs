using System;
using System.Threading.Tasks;
using RepeatingWords.Helpers.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RepeatingWords.Helpers
{
   public class LikeApplication
   {
       private static string uriAndroidLike = "https://play.google.com/store/apps/details?id=cardsofwords.cardsofwords";
       //private static string uriUWPLike = "https://www.microsoft.com/store/apps/9n55bwkgshnf";
       //private static string uriIOSLike = "";

        public static async Task Like(IDialogService _dialogService)
        {
            bool action = await _dialogService.ShowConfirmAsync(Resource.MessagePleaseReview, "", Resource.ButtonSendReview, Resource.ModalActCancel);
            if (action)
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                    {
                        if (await Launcher.CanOpenAsync(uriAndroidLike))
                            await Launcher.OpenAsync(uriAndroidLike);
                        else
                            _dialogService.ShowToast(Resource.ModalCheckNet);
                        break;
                    }
                    //case Device.UWP:
                    //{
                    //    Device.OpenUri(new Uri("https://www.microsoft.com/store/apps/9n55bwkgshnf"));
                    //    break;
                    //}
                    //case Device.iOS:
                    //{
                      
                    //    break;
                    //}
                }
            }
        }
    }
}
