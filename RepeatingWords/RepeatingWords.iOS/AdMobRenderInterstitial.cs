using System;
using RepeatingWords.iOS;
using Xamarin.Forms;
using Foundation;
using Google.MobileAds;
using UIKit;

[assembly: Dependency(typeof(AdMobRendererInterstitial))]

namespace RepeatingWords.iOS
{
    public class AdMobRendererInterstitial:IAdmobInterstitial
    {
        private Interstitial _interstitial; 
        public AdMobRendererInterstitial()
        {
            LoadAd();
            _interstitial.ScreenDismissed += (s, e) => LoadAd();
        }

        private void LoadAd()
        {
            //todo: add id adbvertizing for ios
            _interstitial = new Interstitial("ca-app-pub-5993977371632312/9672263359");
            var request = Request.GetDefaultRequest();
#pragma warning disable CS0618 // Type or member is obsolete
         //   request.TestDevices = new string[] { "3B68FB49-8490-48E8-BBC3-41A1BA2D41B7", "GADSimulator" };
#pragma warning restore CS0618 // Type or member is obsolete
            _interstitial.LoadRequest(request);
        }

        public void Show(string adUnit)
        {
            if(_interstitial.IsReady)
            {
                var view = GetVisibleViewController();
                _interstitial.PresentFromRootViewController(view);
            }
        }

        private UIViewController GetVisibleViewController()
        {
            var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (rootController.PresentedViewController == null)
                return rootController;

            if (rootController.PresentedViewController is UINavigationController)
            {
                return ((UINavigationController)rootController.PresentedViewController).VisibleViewController;
            }

            if (rootController.PresentedViewController is UITabBarController)
            {
                return ((UITabBarController)rootController.PresentedViewController).SelectedViewController;
            }

            return rootController.PresentedViewController;
        }
    }
}
