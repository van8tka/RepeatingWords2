using System;
using System.Diagnostics;
using Android.Gms.Ads;
using NLog;
using Xamarin.Forms;
using RepeatingWords.Droid;
using Xamarin.Forms.Internals;

[assembly: Dependency(typeof(AdMobRendererInterstitial))]

namespace RepeatingWords.Droid
{
    class AdMobRendererInterstitial : IAdmobInterstitial
    {
        InterstitialAd _ad;

        public void Show(string adUnit)
        {
            var context = Android.App.Application.Context;
            _ad = new InterstitialAd(context);
            _ad.AdUnitId = adUnit;
            var intlistener = new InterstitialAdListener(_ad);
            intlistener.OnAdLoaded();
            _ad.AdListener = intlistener;
            var requestbuilder = new AdRequest.Builder();
            _ad.LoadAd(requestbuilder.Build());
        }
    }
}

